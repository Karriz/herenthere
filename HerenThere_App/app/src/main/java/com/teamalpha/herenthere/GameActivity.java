package com.teamalpha.herenthere;

import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.location.LocationManager;
import android.os.Build;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v4.app.FragmentActivity;
import android.os.Bundle;
import android.support.v4.content.ContextCompat;
import android.util.Log;
import android.view.View;

import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.api.GoogleApiClient;
import android.location.LocationListener;
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


import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

public class GameActivity extends FragmentActivity implements
        OnMapReadyCallback,
        GoogleApiClient.ConnectionCallbacks,
        GoogleApiClient.OnConnectionFailedListener,
        GoogleMap.OnMarkerDragListener,
        GoogleMap.OnMapLongClickListener,
        GoogleMap.OnMapClickListener,
        View.OnClickListener,
        LocationListener{

    private static final String TAG = "GameActivity";

    private GoogleMap mMap;
    private GoogleApiClient googleApiClient;

    private Location playerLocation;
    private List<Marker> playerMarkers;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);

        Intent intent = getIntent();
        String name = intent.getStringExtra(CommonMethods.EXTRA_MESSAGE);
        CommonMethods.showToastMessage(this,"Welcome, player "+name);

        //Initializing googleapi client
        googleApiClient = new GoogleApiClient.Builder(this)
                .addConnectionCallbacks(this)
                .addOnConnectionFailedListener(this)
                .addApi(LocationServices.API)
                .build();

        playerMarkers = new ArrayList<Marker>();

        HTTPGetTask httpGetTask = new HTTPGetTask();

        try {
            httpGetTask.run("http://192.168.11.12:8000", this);
        } catch (IOException e) {
            e.printStackTrace();
        }

        HTTPPostTask httpPostTask = new HTTPPostTask();

        try {
            httpPostTask.post("http://192.168.11.12:8000", "body", this);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    protected void onStart() {
        googleApiClient.connect();
        super.onStart();
    }

    @Override
    protected void onStop() {
        googleApiClient.disconnect();
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
        mMap.setMyLocationEnabled(true);
        mMap.setMapType(GoogleMap.MAP_TYPE_HYBRID);

        mMap.setMinZoomPreference(14.0f);
        mMap.setMaxZoomPreference(21.0f);

        mMap.setLatLngBoundsForCameraTarget(new LatLngBounds(new LatLng(65.008692, 25.451551),new LatLng(65.016454, 25.484114)));

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

        if ( Build.VERSION.SDK_INT >= 23 &&
                ContextCompat.checkSelfPermission( this, android.Manifest.permission.ACCESS_FINE_LOCATION ) != PackageManager.PERMISSION_GRANTED &&
                ContextCompat.checkSelfPermission( this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {

            Log.e(TAG, "No location permission");
        }
        else {
            // Acquire a reference to the system Location Manager
            LocationManager locationManager = (LocationManager) this.getSystemService(this.LOCATION_SERVICE);

            locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 100, 1, this);
        }

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
        Log.d(TAG,"Location changed: "+location.getLatitude()+", "+location.getLongitude());
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
}
