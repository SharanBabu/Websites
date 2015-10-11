/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.ars.source;
import java.sql.*;

public class MySQL {
    public static Connection connect(){
    try{
        Class.forName("com.mysql.jdbc.Driver").newInstance();
        return DriverManager.getConnection("jdbc:mysql://localhost/airline","root","root");
    } catch (Exception e){
        return null;
    }            
}
public static boolean close(Connection c)
{
    try{
        c.close();
        return true;
    }catch(Exception e){
        return false;
    }
}
}
