package com.teamalpha.herenthere;

import android.content.Context;
import android.widget.Toast;

import org.json.JSONObject;

/**
 * Created by Karri on 15.10.2016.
 */

public class CommonMethods {

    private static final String TAG = "CommonMethods";

    public static void showToastMessage(Context context,String msg) {
        int duration = Toast.LENGTH_SHORT;

        Toast toast = Toast.makeText(context, msg, duration);
        toast.show();
    }

}
