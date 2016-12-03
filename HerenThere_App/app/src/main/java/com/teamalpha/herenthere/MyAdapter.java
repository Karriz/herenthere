package com.teamalpha.herenthere;

import android.content.Context;
import android.graphics.Color;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Karri on 26.11.2016.
 */

public class MyAdapter extends ArrayAdapter {
    private final Context context;
    private final List<String> values;

    private List<Integer> colors = new ArrayList<Integer>();

    public MyAdapter(Context context, int r1, int r2, List<String> values) {
        super(context, r1, r2, values);
        this.context = context;
        this.values = values;

        colors.add(Color.RED);
        colors.add(Color.parseColor("#ffa500"));
        colors.add(Color.parseColor("#00b200"));
        colors.add(Color.BLUE);
        colors.add(Color.parseColor("#007fff"));
        colors.add(Color.CYAN);
        colors.add(Color.YELLOW);

    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        View view = super.getView(position, convertView, parent);
        if (position % 2 == 1) {
            view.setBackgroundColor(Color.BLUE);
        } else {
            view.setBackgroundColor(Color.CYAN);
        }

        if (position < colors.size()) {
            view.setBackgroundColor(colors.get(position));
        }
        else {
            view.setBackgroundColor(colors.get(colors.size()-1));
        }

        return view;
    }
}
