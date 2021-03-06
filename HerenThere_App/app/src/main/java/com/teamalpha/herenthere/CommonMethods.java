package com.teamalpha.herenthere;

import android.content.Context;
import android.os.CountDownTimer;
import android.util.Log;
import android.widget.Toast;

import org.json.JSONObject;

/**
 * Created by Karri on 15.10.2016.
 */

public class CommonMethods {

    public static boolean sensorInitialized = false;
    private static final String TAG = "CommonMethods";
    public static GameActivity game = null;

    private static CountDownTimer gameUpdateTimer = null;



    public static void showToastMessage(Context context,String msg) {
        int duration = Toast.LENGTH_SHORT;

        Toast toast = Toast.makeText(context, msg, duration);
        toast.show();
    }

    public static void updateActivityState(boolean moving) {
        if (game != null) {
            if (game.gameState != "ended") {
                Log.d(TAG, "Player is moving:" + moving);
                game.moving = moving;
                if (moving) {
                    if (game.gameState != "moving") {
                        game.changeState("moving");
                    }
                } else if (game.gameState == "moving") {
                    game.changeState("locations");
                }
            }
        }
    }

    public static void stopGameLoop() {
        if (gameUpdateTimer != null) gameUpdateTimer.cancel();
    }

    public static void startGameLoop() {
        if (gameUpdateTimer != null) gameUpdateTimer.cancel();

        gameUpdateTimer = new CountDownTimer(5000, 500) {
            @Override
            public void onTick(long millisUntilFinished) {
                if (game.endTime != 0) {
                    long diff = game.endTime - System.currentTimeMillis();
                    if (diff > 0) {
                        //Log.d(TAG, "time left: "+diff);
                    }
                    else {
                        game.changeState("ended");
                    }
                }
                else {
                    Log.d(TAG, "Game end time has not been set!");
                }
            }

            @Override
            public void onFinish() {
                if (game.gameState != "hiding") {
                    game.updateMarkers();
                }
                game.updateScores();
                startGameLoop();
            }
        };
        gameUpdateTimer.start();
    }

}
