package com.teamalpha.herenthere;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Created by Karri on 8.11.2016.
 */

//define callback interface
public interface CallbackInterface {
    void onResponse(JSONObject result) throws JSONException;
    void onError() throws JSONException;
}
