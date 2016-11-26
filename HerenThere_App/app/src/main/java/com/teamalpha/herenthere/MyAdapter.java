package com.teamalpha.herenthere;

import android.content.Context;
import android.graphics.Color;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import java.util.List;

/**
 * Created by Karri on 26.11.2016.
 */

public class MyAdapter extends ArrayAdapter {
    private final Context context;
    private final List<String> values;

    public MyAdapter(Context context, int r1, int r2, List<String> values) {
        super(context, r1, r2, values);
        this.context = context;
        this.values = values;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        View view = super.getView(position, convertView, parent);
        if (position % 2 == 1) {
            view.setBackgroundColor(Color.BLUE);
        } else {
            view.setBackgroundColor(Color.CYAN);
        }

        return view;
    }
}
