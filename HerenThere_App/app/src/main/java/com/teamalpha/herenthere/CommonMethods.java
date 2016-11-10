package com.teamalpha.herenthere;

import android.content.Context;
import android.util.Log;
import android.widget.Toast;

import org.json.JSONObject;

/**
 * Created by Karri on 15.10.2016.
 */

public class CommonMethods {

    private static final String TAG = "CommonMethods";
    public static GameActivity game = null;

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

}
