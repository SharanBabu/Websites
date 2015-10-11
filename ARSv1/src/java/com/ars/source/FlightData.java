/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.ars.source;
import java.util.ArrayList;
import java.util.Arrays;
/**
 *
 * @author sharan
 */
public class FlightData {
    public String flightNumber;
    public String weekdays;
    public byte weekdaysByte;    
    public byte arrivingWeekdaysByte;
    public String departAirportCode;
    public String departAirportName;
    public String departTime;
    public String arrivalAirportCode;
    public String arrivalAirportName;
    public String arrivalTime;   
    
    public byte getWeekDaysByte(String weekdays)
    {
        byte weekByte = 0x00;
        String[] weekdaysArray = weekdays.split("_");
        ArrayList<String> arrayList = new ArrayList<String>(Arrays.asList(weekdaysArray));
        if(arrayList.contains("Sun")){
            weekByte |= (1 << 0);
        }
        if(arrayList.contains("Mon")){
            weekByte |= (1 << 1);
        }
        if(arrayList.contains("Tue")){
            weekByte |= (1 << 2);
        }
        if(arrayList.contains("Wed")){
            weekByte |= (1 << 3);
        }
        if(arrayList.contains("Thu")){
            weekByte |= (1 << 4);
        }
        if(arrayList.contains("Fri")){
            weekByte |= (1 << 5);
        }
        if(arrayList.contains("Sat")){
            weekByte |= (1 << 6);
        }
        return weekByte;
    }
}
