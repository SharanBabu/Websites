/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.ars.source;

import com.google.gson.Gson;
import java.io.IOException;
import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.Time;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

/**
 *
 * @author sharan
 */
@WebServlet(name = "SearchFlights", urlPatterns = {"/SearchFlights"})
public class SearchFlights extends HttpServlet {
    
    private void getAllDirectFlights(Connection cn, String from, String to, List<Flights> searchRes)
    {
        EntityFactory ef = new EntityFactory(cn, "select f1.flight_number f1FlightNo, f1.weekdays f1Weekdays,\n" +
"f1.departure_airport_code f1DepartCode, a1.name f1DepartName, \n" +
"f1.scheduled_departure_time f1DepartTime, f1.arrival_airport_code f1ArrivalCode, a2.name f1ArrivalName, f1.scheduled_arrival_time f1ArrivalTime\n" +
"from flight f1 \n" +
"join airport a1 on f1.departure_airport_code = a1.airport_code \n" +
"join airport a2 on f1.arrival_airport_code = a2.airport_code \n" +
"where f1.departure_airport_code = ? and f1.arrival_airport_code = ?");
        try{
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{from, to});            
            for (Map<String, Object> resSet1 : resSet) {
                List<FlightData> temp = new ArrayList<>();
                FlightData f1 = new FlightData();
                f1.flightNumber = (String) (resSet1.get("f1flightno"));
                f1.departAirportCode = (String) (resSet1.get("f1departcode"));
                f1.departAirportName = (String) (resSet1.get("f1departname"));
                f1.departTime = (resSet1.get("f1departtime")).toString();
                f1.arrivalAirportCode = (String) (resSet1.get("f1arrivalcode"));
                f1.arrivalAirportName = (String) (resSet1.get("f1arrivalname"));
                f1.arrivalTime = (resSet1.get("f1arrivaltime")).toString();
                f1.weekdays = (String) (resSet1.get("f1weekdays"));
                f1.weekdaysByte = f1.getWeekDaysByte(f1.weekdays);
                temp.add(f1);
                Flights flList = new Flights();
                flList.flightList = temp;
                searchRes.add(flList);                
            }              
        }catch(Exception ex){            
        }
    }
    private void getAllOneStopFlights(Connection cn, String from, String to, List<Flights> searchRes)
    {
        EntityFactory ef = new EntityFactory(cn, "select f1.flight_number f1FlightNo, f1.weekdays f1Weekdays,\n"+
"f1.departure_airport_code f1DepartCode, a1.name f1DepartName,\n" +
"f1.scheduled_departure_time f1DepartTime, f1.arrival_airport_code f1ArrivalCode, a2.name f1ArrivalName, f1.scheduled_arrival_time f1ArrivalTime,\n" +
"f2.flight_number f2FlightNo, f2.weekdays f2Weekdays, f2.departure_airport_code f2DepartCode, a3.name f2DepartName,\n" +
"f2.scheduled_departure_time f2DepartTime, f2.arrival_airport_code f2ArrivalCode, a4.name f2ArrivalName, f2.scheduled_arrival_time f2ArrivalTime\n" +
"from flight f1 join flight f2 on f1.arrival_airport_code = f2.departure_airport_code\n" +
"join airport a1 on f1.departure_airport_code = a1.airport_code\n" +
"join airport a2 on f1.arrival_airport_code = a2.airport_code\n" +
"join airport a3 on f2.departure_airport_code = a3.airport_code\n" +
"join airport a4 on f2.arrival_airport_code = a4.airport_code\n" +
"where f1.departure_airport_code = ? and f2.arrival_airport_code = ?\n" +
"and SUBTIME(f2.scheduled_departure_time, f1.scheduled_arrival_time) > '01:00:00'");

        try{
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{from, to});            
            for (Map<String, Object> resSet1 : resSet) {
                List<FlightData> temp = new ArrayList<>();
                FlightData f1 = new FlightData();
                f1.flightNumber = (String) (resSet1.get("f1flightno"));
                f1.departAirportCode = (String) (resSet1.get("f1departcode"));
                f1.departAirportName = (String) (resSet1.get("f1departname"));
                f1.departTime = (resSet1.get("f1departtime")).toString();
                f1.arrivalAirportCode = (String) (resSet1.get("f1arrivalcode"));
                f1.arrivalAirportName = (String) (resSet1.get("f1arrivalname"));
                f1.arrivalTime = (resSet1.get("f1arrivaltime")).toString();
                f1.weekdays = (String) (resSet1.get("f1weekdays"));
                f1.weekdaysByte = f1.getWeekDaysByte(f1.weekdays);
                temp.add(f1);
                FlightData f2 = new FlightData();
                f2.flightNumber = (String) (resSet1.get("f2flightno"));
                f2.departAirportCode = (String) (resSet1.get("f2departcode"));
                f2.departAirportName = (String) (resSet1.get("f2departname"));
                f2.departTime = (resSet1.get("f2departtime")).toString();
                f2.arrivalAirportCode = (String) (resSet1.get("f2arrivalcode"));
                f2.arrivalAirportName = (String) (resSet1.get("f2arrivalname"));
                f2.arrivalTime = (resSet1.get("f2arrivaltime")).toString();
                f2.weekdays = (String) (resSet1.get("f2weekdays"));
                f2.weekdaysByte = f2.getWeekDaysByte(f2.weekdays);
                temp.add(f2);                
                byte finalWeekdaysByte = computeFinalWeekdays(temp);
                if(finalWeekdaysByte > 0x00){//iscompatible
                    setWeekdaysByte(temp, finalWeekdaysByte);
                    Flights flList = new Flights();
                    flList.flightList = temp;
                    searchRes.add(flList);
                }
            }              
        }catch(Exception ex){            
        }
    }
    private void getAllTwoStopFlights(Connection cn, String from, String to, List<Flights> searchRes)
    {
        EntityFactory ef = new EntityFactory(cn, "select f1.flight_number f1FlightNo, f1.weekdays f1Weekdays,\n" +
"f1.departure_airport_code f1DepartCode, a1.name f1DepartName, \n" +
"f1.scheduled_departure_time f1DepartTime, f1.arrival_airport_code f1ArrivalCode, a2.name f1ArrivalName, f1.scheduled_arrival_time f1ArrivalTime, \n" +
"f2.flight_number f2FlightNo, f2.weekdays f2Weekdays, f2.departure_airport_code f2DepartCode, a3.name f2DepartName, \n" +
"f2.scheduled_departure_time f2DepartTime, f2.arrival_airport_code f2ArrivalCode, a4.name f2ArrivalName, f2.scheduled_arrival_time f2ArrivalTime,\n" +
"f3.flight_number f3FlightNo, f3.weekdays f3Weekdays, f3.departure_airport_code f3DepartCode, a5.name f3DepartName, \n" +
"f3.scheduled_departure_time f3DepartTime, f3.arrival_airport_code f3ArrivalCode, a6.name f3ArrivalName, f3.scheduled_arrival_time f3ArrivalTime\n" +
"from flight f1 join flight f2 on f1.arrival_airport_code = f2.departure_airport_code\n" +
"join flight f3 on f2.arrival_airport_code = f3.departure_airport_code\n" +
"join airport a1 on f1.departure_airport_code = a1.airport_code \n" +
"join airport a2 on f1.arrival_airport_code = a2.airport_code \n" +
"join airport a3 on f2.departure_airport_code = a3.airport_code \n" +
"join airport a4 on f2.arrival_airport_code = a4.airport_code \n" +
"join airport a5 on f3.departure_airport_code = a5.airport_code \n" +
"join airport a6 on f3.arrival_airport_code = a6.airport_code\n" +
"where f1.departure_airport_code = ? and f3.arrival_airport_code = ?\n" +
"and SUBTIME(f2.scheduled_departure_time, f1.scheduled_arrival_time) > '01:00:00' \n" +
"and SUBTIME(f3.scheduled_departure_time, f2.scheduled_arrival_time) > '01:00:00' and f1.departure_airport_code != f2.arrival_airport_code and f2.departure_airport_code != f3.arrival_airport_code");
        try{
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{from, to});            
            for (Map<String, Object> resSet1 : resSet) {
                List<FlightData> temp = new ArrayList<>();
                FlightData f1 = new FlightData();
                f1.flightNumber = (String) (resSet1.get("f1flightno"));
                f1.departAirportCode = (String) (resSet1.get("f1departcode"));
                f1.departAirportName = (String) (resSet1.get("f1departname"));
                f1.departTime = (resSet1.get("f1departtime")).toString();
                f1.arrivalAirportCode = (String) (resSet1.get("f1arrivalcode"));
                f1.arrivalAirportName = (String) (resSet1.get("f1arrivalname"));
                f1.arrivalTime = (resSet1.get("f1arrivaltime")).toString();
                f1.weekdays = (String) (resSet1.get("f1weekdays"));
                f1.weekdaysByte = f1.getWeekDaysByte(f1.weekdays);
                temp.add(f1);
                FlightData f2 = new FlightData();
                f2.flightNumber = (String) (resSet1.get("f2flightno"));
                f2.departAirportCode = (String) (resSet1.get("f2departcode"));
                f2.departAirportName = (String) (resSet1.get("f2departname"));
                f2.departTime = (resSet1.get("f2departtime")).toString();
                f2.arrivalAirportCode = (String) (resSet1.get("f2arrivalcode"));
                f2.arrivalAirportName = (String) (resSet1.get("f2arrivalname"));
                f2.arrivalTime = (resSet1.get("f2arrivaltime")).toString();
                f2.weekdays = (String) (resSet1.get("f2weekdays"));
                f2.weekdaysByte = f2.getWeekDaysByte(f2.weekdays);
                temp.add(f2);
                FlightData f3 = new FlightData();
                f3.flightNumber = (String) (resSet1.get("f3flightno"));
                f3.departAirportCode = (String) (resSet1.get("f3departcode"));
                f3.departAirportName = (String) (resSet1.get("f3departname"));
                f3.departTime = (resSet1.get("f3departtime")).toString();
                f3.arrivalAirportCode = (String) (resSet1.get("f3arrivalcode"));
                f3.arrivalAirportName = (String) (resSet1.get("f3arrivalname"));
                f3.arrivalTime = (resSet1.get("f3arrivaltime")).toString();
                f3.weekdays = (String) (resSet1.get("f3weekdays"));
                f3.weekdaysByte = f3.getWeekDaysByte(f3.weekdays);
                temp.add(f3);
                byte finalWeekdaysByte = computeFinalWeekdays(temp);
                if(finalWeekdaysByte > 0x00){//iscompatible
                    setWeekdaysByte(temp, finalWeekdaysByte);
                    Flights flList = new Flights();
                    flList.flightList = temp;
                    searchRes.add(flList);
                }
            }              
        }catch(Exception ex){            
        }
    }
    private void getAllThreeStopFlights(Connection cn, String from, String to, List<Flights> searchRes)
    {
        EntityFactory ef = new EntityFactory(cn, "select f1.flight_number f1FlightNo, f1.weekdays f1Weekdays,\n" +
"f1.departure_airport_code f1DepartCode, a1.name f1DepartName, \n" +
"f1.scheduled_departure_time f1DepartTime, f1.arrival_airport_code f1ArrivalCode, a2.name f1ArrivalName, f1.scheduled_arrival_time f1ArrivalTime, \n" +
"f2.flight_number f2FlightNo, f2.weekdays f2Weekdays, f2.departure_airport_code f2DepartCode, a3.name f2DepartName, \n" +
"f2.scheduled_departure_time f2DepartTime, f2.arrival_airport_code f2ArrivalCode, a4.name f2ArrivalName, f2.scheduled_arrival_time f2ArrivalTime,\n" +
"f3.flight_number f3FlightNo, f3.weekdays f3Weekdays, f3.departure_airport_code f3DepartCode, a5.name f3DepartName, \n" +
"f3.scheduled_departure_time f3DepartTime, f3.arrival_airport_code f3ArrivalCode, a6.name f3ArrivalName, f3.scheduled_arrival_time f3ArrivalTime,\n" +
"f4.flight_number f4FlightNo, f4.weekdays f4Weekdays, f4.departure_airport_code f4DepartCode, a7.name f4DepartName, \n" +
"f4.scheduled_departure_time f4DepartTime, f4.arrival_airport_code f4ArrivalCode, a8.name f4ArrivalName, f4.scheduled_arrival_time f4ArrivalTime\n" +
"from flight f1 join flight f2 on f1.arrival_airport_code = f2.departure_airport_code\n" +
"join flight f3 on f2.arrival_airport_code = f3.departure_airport_code\n" +
"join flight f4 on f3.arrival_airport_code = f4.departure_airport_code\n" +
"join airport a1 on f1.departure_airport_code = a1.airport_code \n" +
"join airport a2 on f1.arrival_airport_code = a2.airport_code \n" +
"join airport a3 on f2.departure_airport_code = a3.airport_code \n" +
"join airport a4 on f2.arrival_airport_code = a4.airport_code \n" +
"join airport a5 on f3.departure_airport_code = a5.airport_code \n" +
"join airport a6 on f3.arrival_airport_code = a6.airport_code\n" +
"join airport a7 on f4.departure_airport_code = a7.airport_code \n" +
"join airport a8 on f4.arrival_airport_code = a8.airport_code\n" +
"where f1.departure_airport_code = ? and f4.arrival_airport_code = ?\n" +
"and SUBTIME(f2.scheduled_departure_time, f1.scheduled_arrival_time) > '01:00:00' \n" +
"and SUBTIME(f3.scheduled_departure_time, f2.scheduled_arrival_time) > '01:00:00' \n" +
"and SUBTIME(f4.scheduled_departure_time, f3.scheduled_arrival_time) > '01:00:00' and f1.departure_airport_code != f2.arrival_airport_code and f2.departure_airport_code != f3.arrival_airport_code\n" +
"and f3.departure_airport_code != f4.arrival_airport_code");
        try{
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{from, to});            
            for (Map<String, Object> resSet1 : resSet) {
                List<FlightData> temp = new ArrayList<>();
                FlightData f1 = new FlightData();
                f1.flightNumber = (String) (resSet1.get("f1flightno"));
                f1.departAirportCode = (String) (resSet1.get("f1departcode"));
                f1.departAirportName = (String) (resSet1.get("f1departname"));
                f1.departTime = (resSet1.get("f1departtime")).toString();
                f1.arrivalAirportCode = (String) (resSet1.get("f1arrivalcode"));
                f1.arrivalAirportName = (String) (resSet1.get("f1arrivalname"));
                f1.arrivalTime = (resSet1.get("f1arrivaltime")).toString();
                f1.weekdays = (String) (resSet1.get("f1weekdays"));
                f1.weekdaysByte = f1.getWeekDaysByte(f1.weekdays);
                temp.add(f1);
                FlightData f2 = new FlightData();
                f2.flightNumber = (String) (resSet1.get("f2flightno"));
                f2.departAirportCode = (String) (resSet1.get("f2departcode"));
                f2.departAirportName = (String) (resSet1.get("f2departname"));
                f2.departTime = (resSet1.get("f2departtime")).toString();
                f2.arrivalAirportCode = (String) (resSet1.get("f2arrivalcode"));
                f2.arrivalAirportName = (String) (resSet1.get("f2arrivalname"));
                f2.arrivalTime = (resSet1.get("f2arrivaltime")).toString();
                f2.weekdays = (String) (resSet1.get("f2weekdays"));
                f2.weekdaysByte = f2.getWeekDaysByte(f2.weekdays);
                temp.add(f2);
                FlightData f3 = new FlightData();
                f3.flightNumber = (String) (resSet1.get("f3flightno"));
                f3.departAirportCode = (String) (resSet1.get("f3departcode"));
                f3.departAirportName = (String) (resSet1.get("f3departname"));
                f3.departTime = (resSet1.get("f3departtime")).toString();
                f3.arrivalAirportCode = (String) (resSet1.get("f3arrivalcode"));
                f3.arrivalAirportName = (String) (resSet1.get("f3arrivalname"));
                f3.arrivalTime = (resSet1.get("f3arrivaltime")).toString();
                f3.weekdays = (String) (resSet1.get("f3weekdays"));
                f3.weekdaysByte = f3.getWeekDaysByte(f3.weekdays);
                temp.add(f3);
                FlightData f4 = new FlightData();
                f4.flightNumber = (String) (resSet1.get("f4flightno"));
                f4.departAirportCode = (String) (resSet1.get("f4departcode"));
                f4.departAirportName = (String) (resSet1.get("f4departname"));
                f4.departTime = (resSet1.get("f4departtime")).toString();
                f4.arrivalAirportCode = (String) (resSet1.get("f4arrivalcode"));
                f4.arrivalAirportName = (String) (resSet1.get("f4arrivalname"));
                f4.arrivalTime = (resSet1.get("f4arrivaltime")).toString();
                f4.weekdays = (String) (resSet1.get("f4weekdays"));
                f4.weekdaysByte = f4.getWeekDaysByte(f4.weekdays);
                temp.add(f4);
                byte finalWeekdaysByte = computeFinalWeekdays(temp);
                if(finalWeekdaysByte > 0x00){//iscompatible
                    setWeekdaysByte(temp, finalWeekdaysByte);
                    Flights flList = new Flights();
                    flList.flightList = temp;
                    searchRes.add(flList);
                }
            }              
        }catch(Exception ex){            
        }
    }
    private void setWeekdaysByte(List<FlightData> flightList, byte weekDaysByte)
    {
        flightList.get(flightList.size()-1).weekdaysByte = weekDaysByte;
        flightList.get(flightList.size()-1).weekdays = RevEnggWeekDays(weekDaysByte);
        for(int i =flightList.size()-2; i >= 0; i--)
        {
            flightList.get(i).weekdaysByte = computeDepartureWeekdaysByte(flightList.get(i).departTime, flightList.get(i).arrivalTime, flightList.get(i+1).weekdaysByte);
            flightList.get(i).weekdays = RevEnggWeekDays(flightList.get(i).weekdaysByte);
        }
    }
    
    private String RevEnggWeekDays(byte weekDaysByte){
        String res = "";
        if((weekDaysByte & 0x01) > 0){
            res += "Sun,";
        }
        if((weekDaysByte & 0x02) > 0){
            res += "Mon,";
        }
        if((weekDaysByte & 0x04) > 0){
            res += "Tue,";
        }
        if((weekDaysByte & 0x08) > 0){
            res += "Wed,";
        }
        if((weekDaysByte & 0x10) > 0){
            res += "Thu,";
        }
        if((weekDaysByte & 0x20) > 0){
            res += "Fri,";
        }
        if((weekDaysByte & 0x40) > 0){
            res += "Sat,";
        }
        return res.substring(0,res.length()-1);
    }
    private byte computeFinalWeekdays(List<FlightData> flightList){
        byte result = (byte)(computeArrivingWeekdaysByte(flightList.get(0).departTime,flightList.get(0).arrivalTime,flightList.get(0).weekdaysByte) & flightList.get(1).weekdaysByte);
        for(int i = 2; i< flightList.size();i++)
        {
            result = (byte)(computeArrivingWeekdaysByte(flightList.get(i-1).departTime,flightList.get(i-1).arrivalTime,result) & flightList.get(i).weekdaysByte);
        }
        return result;
    }
    private byte computeArrivingWeekdaysByte(String departTime, String arrivingTime, byte departWeekdaysByte){   
          byte arrByte;
          Time dTime = Time.valueOf(departTime);
          Time aTime = Time.valueOf(arrivingTime);
          long ms = aTime.getTime() - dTime.getTime();
          if(ms > 0)//same day
          {
              arrByte = departWeekdaysByte;
          }
          else//next day
          {
              arrByte = (byte)(departWeekdaysByte << 1);
              if((arrByte & 0x80) > 0){//sat was der
                   arrByte |= (1 << 0);//sun is set
                   arrByte &= ~(1 << 7);//8th bit is cleared
              }
          }
          return arrByte;
    }
    private byte computeDepartureWeekdaysByte(String departTime, String arrivingTime, byte arrivingWeekdaysByte){   
          byte arrByte;
          Time dTime = Time.valueOf(departTime);
          Time aTime = Time.valueOf(arrivingTime);
          long ms = aTime.getTime() - dTime.getTime();
          if(ms > 0)//same day
          { 
              arrByte = arrivingWeekdaysByte;
          }
          else//next day
          {
              arrByte = (byte)(arrivingWeekdaysByte >>> 1);
              if((arrivingWeekdaysByte & 0x01) > 0){//sun was der
                   arrByte |= (1 << 6);//sat is set                   
              }
          }
          return arrByte;
    }
    private String getAllflights(String from, String to, int maxStops) {
        try {            
            String res;
            Connection cn = MySQL.connect();
            List<Flights> resSet = new ArrayList();            
            getAllDirectFlights(cn, from, to, resSet);            
            if(resSet.size() < 1){
                if(maxStops > 0){
                    getAllOneStopFlights(cn, from, to, resSet);                
                }
                if(maxStops > 1){
                    getAllTwoStopFlights(cn, from, to, resSet);                
                }
                if(maxStops > 2){
                    getAllThreeStopFlights(cn, from, to, resSet);                
                }
            }
            res = new Gson().toJson(resSet);
            MySQL.close(cn);
            return res;
        } catch (Exception e) {
            return null;
        }
    }
    
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("application/json;charset=UTF-8");
        try (PrintWriter out = response.getWriter()) {            
            String res = getAllflights(request.getParameter("from"),request.getParameter("to"),Integer.parseInt(request.getParameter("maxStops")));            
            out.write(res);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
