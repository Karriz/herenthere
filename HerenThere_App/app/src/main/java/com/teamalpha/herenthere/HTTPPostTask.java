package com.teamalpha.herenthere;

import android.app.Activity;
import android.util.Log;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

public class HTTPPostTask {
    private static final String TAG = "HTTPPostTask";

    public static final MediaType JSON
            = MediaType.parse("application/json; charset=utf-8");

    OkHttpClient client = new OkHttpClient();

    public void post(String url, final CallbackInterface callback) throws IOException {
        RequestBody reqbody = RequestBody.create(null, new byte[0]);

        Request request = new Request.Builder()
                .url(url)
                .header("Content-Length", "0")
                .method("POST",reqbody)
                .build();

        client.newCall(request)
                .enqueue(new Callback() {
                    @Override
                    public void onFailure(final Call call, IOException e) {
                        // Error
                        try {
                            callback.onError();
                        } catch (JSONException e1) {
                            e1.printStackTrace();
                        }
                    }

                    @Override
                    public void onResponse(Call call, final Response response) throws IOException {
                        if (response.code() == 200) {
                            String res = response.body().string();

                            Log.d(TAG, res);

                            if (res.charAt(0) != '{') {
                                res = "{response:"+res+"}";
                            }

                            try {
                                JSONObject obj = new JSONObject(res);
                                callback.onResponse(obj);
                            } catch (JSONException e) {
                                e.printStackTrace();
                            }
                        }
                        else {
                            Log.e(TAG, "Server returned code "+response.code());
                            try {
                                callback.onError();
                            } catch (JSONException e1) {
                                e1.printStackTrace();
                            }
                        }
                    }
                });

    }

}