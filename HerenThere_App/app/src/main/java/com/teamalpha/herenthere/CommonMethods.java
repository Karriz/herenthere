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

    public static void startGameLoop() {
        if (gameUpdateTimer != null) gameUpdateTimer.cancel();

        gameUpdateTimer = new CountDownTimer(2000, 100) {
            @Override
            public void onTick(long millisUntilFinished) {

            }

            @Override
            public void onFinish() {
                game.updateMarkers();
                game.updateScores();
                startGameLoop();
            }
        };
        gameUpdateTimer.start();
    }

}
