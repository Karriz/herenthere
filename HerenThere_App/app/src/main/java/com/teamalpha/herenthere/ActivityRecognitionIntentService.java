package com.teamalpha.herenthere;

import android.app.IntentService;
import android.content.Intent;
import android.util.Log;

import com.google.android.gms.location.ActivityRecognitionResult;
import com.google.android.gms.location.DetectedActivity;

/**
 * Created by Karri on 30.10.2016.
 */

public class ActivityRecognitionIntentService extends IntentService {
    private static final String TAG = "ActivityRecognition";


    public ActivityRecognitionIntentService() {
        super("ActivityRecognitionIntentService");
    }

    /**
     * Creates an IntentService.  Invoked by your subclass's constructor.
     *
     * @param name Used to name the worker thread, important only for debugging.
     */
    public ActivityRecognitionIntentService(String name) {
        super(name);
    }

    /**
     * Called when a new activity detection update is available.
     */
    @Override
    protected void onHandleIntent(Intent intent) {
        Log.d(TAG, "onHandleIntent called");
        //...
        // If the intent contains an update
        if (ActivityRecognitionResult.hasResult(intent)) {
            // Get the update
            ActivityRecognitionResult result =
                    ActivityRecognitionResult.extractResult(intent);

            DetectedActivity mostProbableActivity
                    = result.getMostProbableActivity();

            // Get the confidence % (probability)
            int confidence = mostProbableActivity.getConfidence();

            // Get the type
            int activityType = mostProbableActivity.getType();

            switch (activityType) {
                case DetectedActivity.IN_VEHICLE: {
                    Log.d( "ActivityRecogition", "In Vehicle: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(true);
                    break;
                }
                case DetectedActivity.ON_BICYCLE: {
                    Log.d( "ActivityRecogition", "On Bicycle: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(true);
                    break;
                }
                case DetectedActivity.ON_FOOT: {
                    Log.d( "ActivityRecogition", "On Foot: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(true);
                    break;
                }
                case DetectedActivity.RUNNING: {
                    Log.d( "ActivityRecogition", "Running: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(true);
                    break;
                }
                case DetectedActivity.STILL: {
                    Log.d( "ActivityRecogition", "Still: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(false);
                    break;
                }
                case DetectedActivity.TILTING: {
                    Log.d( "ActivityRecogition", "Tilting: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(false);
                    break;
                }
                case DetectedActivity.WALKING: {
                    Log.d( "ActivityRecogition", "Walking: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(true);
                    break;
                }
                case DetectedActivity.UNKNOWN: {
                    Log.d( "ActivityRecogition", "Unknown: " + mostProbableActivity.getConfidence() );
                    CommonMethods.updateActivityState(false);
                    break;
                }
            }
        }
    }
}
