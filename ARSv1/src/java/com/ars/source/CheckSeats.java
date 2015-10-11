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
import java.text.SimpleDateFormat;
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
@WebServlet(name = "CheckSeats", urlPatterns = {"/CheckSeats"})
public class CheckSeats extends HttpServlet {

    private String getFlightInstance(String flight, String date){
        try {                        
            Connection cn = MySQL.connect();
            EntityFactory ef = new EntityFactory(cn, "select fl.flight_number flNo, fi.date, a.name departName, fl.scheduled_departure_time departTime,\n" +
"a2.name arrivalName, fl.scheduled_arrival_time arrivalTime, fi.number_of_available_seats numSeats from \n" +
"flight_instance fi join flight fl on fl.flight_number = fi.flight_number\n" +
"join Airport a on fl.departure_airport_code = a.airport_code\n" +
"join airport a2 on fl.arrival_airport_code = a2.airport_code\n" +
"where fi.flight_number = ? and fi.date = ?");
            Date d = new SimpleDateFormat("MM/dd/yyyy").parse(date);
            String formattedDate = new SimpleDateFormat("yyyy-MM-dd").format(d);
            Map<String, Object> resSet = ef.findSingle(new String[]{flight,formattedDate});  
            FlightInstance fl = new FlightInstance();
            fl.flightNo = (String) (resSet.get("flno"));
            fl.date = (resSet.get("date")).toString();
            fl.departName = (String)(resSet.get("departname"));
            fl.departTime = (resSet.get("departtime")).toString();
            fl.arrivalName = (String)(resSet.get("arrivalname"));
            fl.arrivalTime = (resSet.get("arrivaltime")).toString();
            fl.numSeatsAvail = (int)resSet.get("numseats");
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
            String res = getFlightInstance(request.getParameter("flight"), request.getParameter("date"));
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
