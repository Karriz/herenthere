package com.teamalpha.herenthere;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

public class MainMenu extends AppCompatActivity {

    private static final String TAG = "MainMenu";
    private EditText nameEdit;
    private int matchId;
    private String matchStr;
    private Button playButton;
    private final Activity _this = this;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_menu);
        nameEdit = (EditText)findViewById(R.id.name_field);
        playButton = (Button)findViewById(R.id.play_button);

        playButton.setEnabled(false);
        nameEdit.setEnabled(false);

        Log.d(TAG, "OnCreate called");

        HTTPGetTask httpGetTask = new HTTPGetTask();

        try {
            httpGetTask.get("http://35.156.7.19/api/HereAndThere/GetOnGoingMatch", new CallbackInterface() {
                @Override
                public void onResponse(JSONObject result) throws JSONException {
                    if (result.has("id")) {
                        matchId = result.getInt("id");
                        matchStr = result.toString();

                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                playButton.setEnabled(true);
                                nameEdit.setEnabled(true);
                                CommonMethods.showToastMessage(_this,"Match found");
                            }
                        });

                        Log.d(TAG, "Match is active, id "+matchId);
                    }
                    else {
                        Log.d(TAG, "Did not find match id!");
                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                CommonMethods.showToastMessage(_this,"Did not find match id!");
                            }
                        });
                    }
                }

                @Override
                public void onError() {
                    Log.e(TAG, "Error getting match!");
                }

            });
        } catch (IOException e) {
            e.printStackTrace();
        }

        if (ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED || ActivityCompat.checkSelfPermission(this, android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            ActivityCompat.requestPermissions(this,
                    new String[]{android.Manifest.permission.ACCESS_FINE_LOCATION},
                    1);
            ActivityCompat.requestPermissions(this,
                    new String[]{Manifest.permission.ACCESS_COARSE_LOCATION},
                    2);
        }

    }

    /** User touches play button **/
    public void playClicked(View view) {

        final String name = nameEdit.getText().toString();
        if (!name.equals("")) {
            Log.d(TAG, "Player name is " + name);
            nameEdit.setEnabled(false);
            playButton.setEnabled(false);

            HTTPPostTask httpPostTask = new HTTPPostTask();

            try {
                httpPostTask.post("http://35.156.7.19/api/HereAndThere/AddPlayerToMatch?name=" + name + "&matchId=" + matchId, new CallbackInterface() {
                    @Override
                    public void onResponse(JSONObject result) throws JSONException {

                        if (result.has("playerId")) {
                            final int playerId = result.getInt("playerId");
                            runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    playButton.setEnabled(true);
                                    //Load the game scene

                                    Intent intent = new Intent(_this, GameActivity.class);
                                    intent.putExtra("MatchStr", matchStr);
                                    intent.putExtra("PlayerName", name);
                                    intent.putExtra("PlayerId", playerId);
                                    startActivity(intent);
                                }
                            });
                        }
                        else {
                            Log.e(TAG, "Response contains no playerId!");

                            runOnUiThread(new Runnable() {
                                @Override
                                public void run() {
                                    nameEdit.setEnabled(true);
                                    playButton.setEnabled(true);
                                    CommonMethods.showToastMessage(_this,"Response contains no playerId!");
                                }
                            });
                        }
                    }

                    @Override
                    public void onError() {
                        Log.e(TAG, "Error while adding player!");

                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                nameEdit.setEnabled(true);
                                playButton.setEnabled(true);
                                CommonMethods.showToastMessage(_this,"Error while adding player!");
                            }
                        });
                    }

                });

            } catch (IOException e) {
                nameEdit.setEnabled(true);
                playButton.setEnabled(true);
                e.printStackTrace();
                CommonMethods.showToastMessage(_this,"IOException!");
            }
        }
        else {
            Log.d(TAG, "No name given!");
            CommonMethods.showToastMessage(this,"Please enter player name");
        }

    }
}
