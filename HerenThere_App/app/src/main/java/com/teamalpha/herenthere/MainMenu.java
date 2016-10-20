package com.teamalpha.herenthere;

import android.content.Context;
import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import com.teamalpha.herenthere.CommonMethods;

public class MainMenu extends AppCompatActivity {

    private static final String TAG = "MainMenu";
    private EditText nameEdit;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_menu);
        nameEdit = (EditText)findViewById(R.id.name_field);

        Log.d(TAG, "OnCreate called");

    }

    /** User touches play button **/
    public void playClicked(View view) {
        String name = nameEdit.getText().toString();
        if (!name.equals("")) {
            Log.d(TAG, "Player name is "+name);
            //Load the game scene

            Intent intent = new Intent(this, GameActivity.class);
            intent.putExtra(CommonMethods.EXTRA_MESSAGE, name);
            startActivity(intent);

        }
        else {
            Log.d(TAG, "No name given!");
            CommonMethods.showToastMessage(this,"Please enter player name");
        }
    }
}
