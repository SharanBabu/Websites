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
@WebServlet(name = "CheckManifest", urlPatterns = {"/CheckManifest"})
public class CheckManifest extends HttpServlet {

    private String getManifest(String flight, String date){
        try {                        
            Connection cn = MySQL.connect();
            EntityFactory ef = new EntityFactory(cn, "select s.customer_name, s.seat_number, s.customer_phone from seat_reservation s where s.flight_number = ? and s.date = ?");
            Date d = new SimpleDateFormat("MM/dd/yyyy").parse(date);
            String formattedDate = new SimpleDateFormat("yyyy-MM-dd").format(d);
            List<Customer> res = new ArrayList();
            List<Map<String, Object>> resSet = ef.findMultiple(new String[]{flight, formattedDate}); 
            for (Map<String, Object> resSet1 : resSet) {
                Customer fl = new Customer();
                fl.customerName = (String) (resSet1.get("customer_name"));
                fl.customerPhone = (String)(resSet1.get("customer_phone"));
                fl.seat = (String)(resSet1.get("seat_number")); 
                res.add(fl);
            }            
            MySQL.close(cn);
            return new Gson().toJson(res);
        } catch (Exception e) {
            return null;
        }
    }
    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/json;charset=UTF-8");
        try (PrintWriter out = response.getWriter()) {
           String res = getManifest(request.getParameter("flight"), request.getParameter("date"));
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
