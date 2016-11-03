package com.teamalpha.herenthere;

import android.util.Log;

import java.io.IOException;

import okhttp3.Call;
import okhttp3.Callback;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;

public class HTTPGetTask {
    private static final String TAG = "HTTPGetTask";

    OkHttpClient client = new OkHttpClient();

    public void run(String url, final GameActivity context) throws IOException {
        Request request = new Request.Builder()
                .url(url)
                .build();

        client.newCall(request)
                .enqueue(new Callback() {
                    @Override
                    public void onFailure(final Call call, IOException e) {
                        // Error

                    }

                    @Override
                    public void onResponse(Call call, final Response response) throws IOException {
                        String res = response.body().string();

                        Log.d(TAG, res);
                        context.testCallBack(res);
                    }
                });

    }

}