package com.teamalpha.herenthere;

import android.app.PendingIntent;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.FragmentActivity;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.view.View;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;

import android.location.LocationListener;

import com.google.android.gms.location.ActivityRecognition;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.UiSettings;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.LatLngBounds;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import android.os.CountDownTimer;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.ProgressBar;
import android.widget.TextView;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.w3c.dom.Text;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.TimeZone;

public class GameActivity extends FragmentActivity implements
        OnMapReadyCallback,
        GoogleApiClient.ConnectionCallbacks,
        GoogleApiClient.OnConnectionFailedListener,
        GoogleMap.OnMarkerDragListener,
        GoogleMap.OnMapLongClickListener,
        GoogleMap.OnMapClickListener,
        View.OnClickListener,
        LocationListener {

    private static final String TAG = "GameActivity";

    private GoogleMap mMap;
    private GoogleApiClient googleApiClient;
    private LocationManager locationManager;

    private Location playerLocation;
    private List<Marker> playerMarkers;

    private ProgressBar progressBar;
    private Button generateButton;
    private ListView scoreList;
    private TextView hintText;

    private CountDownTimer timer = null;

    private JSONObject matchData;

    private int matchId;
    private String playerName;
    private int lastActualLocationId;

    public boolean moving = false;
    public String gameState = "locations"; //"locations", "spotting" or "moving"

    private List<String> playerScores = new ArrayList<String>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        CommonMethods.game = this;

        setContentView(R.layout.activity_game);
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);

        Intent intent = getIntent();
        String matchStr = intent.getStringExtra("MatchStr");
        String playerName = intent.getStringExtra("PlayerName");

        try {
            matchData = new JSONObject(matchStr);

            matchId = matchData.getInt("id");

            JSONArray players = matchData.getJSONArray("players");

            playerScores = new ArrayList<String>();
            for (int i=0;i<players.length();i++) {
                JSONObject player = players.getJSONObject(i);
                playerScores.add(player.getString("name")+": "+player.getInt("score"));
            }

            CommonMethods.showToastMessage(this, "Welcome, player " + playerName);

        } catch (JSONException e) {
            e.printStackTrace();

            CommonMethods.showToastMessage(this, "Match JSON error!");
            Log.e(TAG, "Match JSON error!");
        }

        //Initializing googleapi client
        googleApiClient = new GoogleApiClient.Builder(this)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .addApi(LocationServices.API)
                .addApi(ActivityRecognition.API)
                .build();

        playerMarkers = new ArrayList<Marker>();

        progressBar=(ProgressBar)findViewById(R.id.progressBar);
        generateButton=(Button)findViewById(R.id.generateButton);
        scoreList =(ListView)findViewById(R.id.scoreList);
        hintText = (TextView)findViewById(R.id.hintText);

        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(
                this,
                R.layout.player_item,
                R.id.playerListItem,
                playerScores );

        scoreList.setAdapter(arrayAdapter);

        changeState("locations");
    }

    @Override
    protected void onStart() {
        googleApiClient.connect();

        if (Build.VERSION.SDK_INT >= 23 &&
                ContextCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED &&
                ContextCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {

            Log.e(TAG, "No location permission");
        } else {
            // Acquire a reference to the system Location Manager
            locationManager = (LocationManager) this.getSystemService(this.LOCATION_SERVICE);

            locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, this);
        }

        super.onStart();
    }

    @Override
    protected void onStop() {
        googleApiClient.disconnect();
        if (ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return;
        }
        locationManager.removeUpdates(this);
        super.onStop();
    }


    /**
     * Manipulates the map once available.
     * This callback is triggered when the map is ready to be used.
     * This is where we can add playerMarkers or lines, add listeners or move the camera. In this case,
     * we just add a marker near Sydney, Australia.
     * If Google Play services is not installed on the device, the user will be prompted to install
     * it inside the SupportMapFragment. This method will only be triggered once the user has
     * installed Google Play services and returned to the app.
     */
    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
        mMap.setOnMapClickListener(this);

        if (ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider calling
            //    ActivityCompat#requestPermissions
            // here to request the missing permissions, and then overriding
            //   public void onRequestPermissionsResult(int requestCode, String[] permissions,
            //                                          int[] grantResults)
            // to handle the case where the user grants the permission. See the documentation
            // for ActivityCompat#requestPermissions for more details.
            return;
        }
        mMap.setMyLocationEnabled(true);
        mMap.setMapType(GoogleMap.MAP_TYPE_HYBRID);

        mMap.setMinZoomPreference(14.0f);
        mMap.setMaxZoomPreference(21.0f);

        try {
            JSONArray boundaries = matchData.getJSONArray("boundaries");
            JSONObject lowerLeft = boundaries.getJSONObject(0);
            JSONObject upperRight = boundaries.getJSONObject(1);

            mMap.setLatLngBoundsForCameraTarget(new LatLngBounds(new LatLng(lowerLeft.getDouble("latitude"), lowerLeft.getDouble("longitude")),new LatLng(upperRight.getDouble("latitude"), upperRight.getDouble("longitude"))));

        } catch (JSONException e) {
            e.printStackTrace();
        }


        UiSettings uiSettings = mMap.getUiSettings();
        uiSettings.setTiltGesturesEnabled(false);
        uiSettings.setRotateGesturesEnabled(false);
        uiSettings.setCompassEnabled(false);

        mMap.setOnMarkerClickListener( new GoogleMap.OnMarkerClickListener()
        {
            @Override
            public boolean onMarkerClick ( Marker marker )
            {
                // do nothing
                return true;
            }
        });

        moveMap(new LatLng(65.016667,25.466667));

    }

    //Function to move the map
    private void moveMap(LatLng latlng) {

        if (latlng != null) {
            double latitude = latlng.latitude;
            double longitude = latlng.longitude;

            Log.d(TAG, "Moving map to: "+latitude + " ," + longitude);

            //Moving the camera
            mMap.moveCamera(CameraUpdateFactory.newLatLng(latlng));

            //Setting zoom to 18
            mMap.animateCamera(CameraUpdateFactory.zoomTo(18));
        }
    }

    //Function to add a marker on the map
    private Marker addMarker(LatLng latlng) {
        //Adding marker to map
        Marker newMarker = mMap.addMarker(new MarkerOptions()
                .position(latlng)); //setting position

        playerMarkers.add(newMarker);
        return newMarker;
    }

    //Function to remove all playerMarkers
    private void removeMarkers() {
        for (int i=playerMarkers.size()-1; i>=0; i--) {
            playerMarkers.get(i).remove();
            playerMarkers.remove(i);
        }
    }

    @Override
    public void onClick(View v) {

    }

    @Override
    public void onConnected(@Nullable Bundle bundle) {
        Log.d(TAG,"onConnected called");
        Intent intent = new Intent(this, ActivityRecognitionIntentService.class);
        PendingIntent pendingIntent = PendingIntent.getService(this, 0, intent, PendingIntent.FLAG_UPDATE_CURRENT);

        ActivityRecognition.ActivityRecognitionApi.requestActivityUpdates(
                googleApiClient,
                0 /* detection interval */,
                pendingIntent);
    }

    @Override
    public void onConnectionSuspended(int i) {

    }

    @Override
    public void onConnectionFailed(@NonNull ConnectionResult connectionResult) {

    }

    @Override
    public void onMapLongClick(LatLng latLng) {

    }

    @Override
    public void onMarkerDragStart(Marker marker) {

    }

    @Override
    public void onMarkerDrag(Marker marker) {

    }

    @Override
    public void onMarkerDragEnd(Marker marker) {

    }

    public void onLocationChanged(Location location) {
        //moveMap(new LatLng(location.getLatitude(),location.getLongitude()));

        playerLocation = location;

        String timeStamp = String.format("%tFT%<tTZ",
                Calendar.getInstance(TimeZone.getTimeZone("Z")));

        Log.d(TAG,"Time: "+timeStamp+", Location changed: "+location.getLatitude()+", "+location.getLongitude());


        HTTPGetTask httpGetTask = new HTTPGetTask();

        try {
            httpGetTask.get("http://35.156.7.19/api/HereAndThere/GetOnGoingMatch", new CallbackInterface() {
                @Override
                public void onResponse(JSONObject result) throws JSONException {


                    runOnUiThread(new Runnable() {
                        @Override
                        public void run() {

                        }
                    });


                }

                @Override
                public void onError() {

                }

            });
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onStatusChanged(String provider, int status, Bundle extras) {

    }

    @Override
    public void onProviderEnabled(String provider) {

    }

    @Override
    public void onProviderDisabled(String provider) {

    }

    @Override
    public void onMapClick(LatLng latLng) {
        Log.d(TAG,"Map was clicked at position: "+latLng.latitude+" ,"+latLng.longitude);
        addMarker(latLng);
    }

    public void testCallBack(final String msg) {
        final GameActivity context = this;
        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                CommonMethods.showToastMessage(context,msg);
            }
        });
    }

    public void changeState(String state) {
        switch (state) {
            case "locations":
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        gameState = "locations";

                        if (timer != null) {
                            timer.cancel();
                        }

                        hintText.setText("Create locations to hide yourself!");

                        timer = new CountDownTimer(30000, 1) {
                            public void onTick(long millisUntilFinished) {
                                int progress = 1000* (int)millisUntilFinished / 30000;
                                progressBar.setProgress(progress);
                            }
                            public void onFinish() {
                                changeState("spotting");
                            }
                        };
                        timer.start();
                    }
                });
                break;
            case "spotting":
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        gameState = "spotting";

                        if (timer != null) {
                            timer.cancel();
                        }

                        hintText.setText("Spot other players, or move");

                    }
                });

                break;
            case "moving":
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        gameState = "moving";

                        if (timer != null) {
                            timer.cancel();
                        }

                        hintText.setText("You can keep moving");

                        timer = new CountDownTimer(30000, 1) {
                            public void onTick(long millisUntilFinished) {
                                int progress = 1000* (int)millisUntilFinished / 30000;
                                progressBar.setProgress(progress);
                            }
                            public void onFinish() {
                                hintText.setText("You are visible! Stop!");
                            }
                        };
                        timer.start();
                    }
                });

                break;
        }
    }
}
