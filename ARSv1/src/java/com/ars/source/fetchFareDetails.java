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
import java.util.ArrayList;
import java.util.Date;
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
@WebServlet(name = "fetchFareDetails", urlPatterns = {"/fetchFareDetails"})
public class fetchFareDetails extends HttpServlet {

    private List<FareInfo> getAllFares(String flight, Connection cn){
        try {                                    
            EntityFactory ef = new EntityFactory(cn, "select f.fare_code farecode, f.amount, f.restrictions from fare f where f.flight_number = ?");
            List<FareInfo> res = new ArrayList();
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{flight}); 
            for (Map<String, Object> resSet1 : resSet) {
                FareInfo fl = new FareInfo();
                fl.fareCode = (String) (resSet1.get("farecode"));
                fl.amount = (double)(resSet1.get("amount"));
                fl.restrictions = (String)(resSet1.get("restrictions")); 
                res.add(fl);
            }                                   
            return res;
        } catch (Exception e) {
            return null;
        }
    }
    private String getFareDetails(String flight){
        try {                        
            Connection cn = MySQL.connect();
            EntityFactory ef = new EntityFactory(cn, "select fl.flight_number flNo, a.name departName,a.airport_code departcode, fl.scheduled_departure_time departTime,\n" +
"a2.name arrivalName,a2.airport_code arrivalcode, fl.scheduled_arrival_time arrivalTime, fl.weekdays from \n" +
"flight fl join Airport a on fl.departure_airport_code = a.airport_code\n" +
"join airport a2 on fl.arrival_airport_code = a2.airport_code\n" +
"where fl.flight_number = ?");
            
            Map<String, Object> resSet = ef.findSingle(new String[]{flight});  
            fareDetails fl = new fareDetails();
            fl.flightNumber = (String) (resSet.get("flno"));
            fl.weekdays = (String)(resSet.get("weekdays"));
            fl.arrivalAirportCode = (String)(resSet.get("arrivalcode"));
            fl.arrivalAirportName = (String)(resSet.get("arrivalname"));
            fl.arrivalTime = (resSet.get("arrivaltime")).toString();
            fl.departAirportCode = (String)(resSet.get("departcode"));
            fl.departAirportName = (String)resSet.get("departname");
            fl.departTime = (resSet.get("departtime")).toString();
            fl.fareDetails = getAllFares(flight, cn);
            String res = new Gson().toJson(fl);            
            MySQL.close(cn);
            return res;
        } catch (Exception e) {
            return null;
        }
    }
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/json;charset=UTF-8");
        try (PrintWriter out = response.getWriter()) { 
            String res = getFareDetails(request.getParameter("flight"));
            out.write(res);
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
