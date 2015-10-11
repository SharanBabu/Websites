<%-- 
    Document   : index
    Created on : Feb 24, 2015, 2:20:02 PM
    Author     : sharan
--%>

<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Airline Reservation System</title>
        <script src="js/jquery.js" type="text/javascript"></script>
        <script src="js/jquery-ui.js" type="text/javascript"></script>
        <script src="js/jquery.tmpl.js" type="text/javascript"></script>      
        <script type="text/javascript" src="js/index.js"></script>
        <link rel ="stylesheet" type="text/css" href="css/jquery-ui.css"/>
        <link rel ="stylesheet" type="text/css" href="css/index.css"/> 
    </head>
    <body>
        <div class="bannerhead">
            <div class="logoDiv"><span class="tChar">T</span>ravelbook</div>
            <div class="welcomeMsg"> Welcome guest!</div>
        </div>
        <div class="optionsDiv">
            <table cellspacing = "30" cellpadding = 0>
                <tr><td><a href="#" class="link" onclick="displayMain('searchFlights')">Search Flights</a></td></tr>                
                <tr><td><a href="#" class="link" onclick="displayMain('checkSeats')">Check Seats</a></td></tr>
                <tr><td><a href="#" class="link" onclick="displayMain('checkFares')">Check Fares</a></td></tr>
                <tr><td><a href="#" class="link" onclick="displayMain('checkManifest')">Check Manifest</a></td></tr>
                <tr><td><a href="#" class="link" onclick="displayMain('searchPassenger')">Search Passenger</a></td></tr>
            </table>
        </div>
        <div class="tabContainer" id="tabDiv">
            <div id="searchFlights" style="display: block;">
                <div class="searchDiv">                    
                    <div class="searchContent">
                        <div class="fromSearch">
                            <div class="textBoxHeading">
                                <span>From</span>
                            </div>
                            <div class="searchBox">
                                <select id="fromAirports" class="selectBox"><option></option></select>
                            </div>
                        </div>
                        <div class="toSearch">
                            <div class="textBoxHeading">
                                <span>To</span>
                            </div>
                            <div class="searchBox">
                                <select id="toAirports" class="selectBox"><option></option></select>
                            </div>
                        </div> 
                        <div class="maxStopsDiv"> 
                            <div class="textBoxHeading">
                                <span>Max Stops</span>
                            </div>
                            <div class="numSelectBox">
                                <input type="number" min="0" max="3" step="1" value="0" class="numSelect ui-corner-all" id="maxStops"/>
                            </div>
                        </div>
                    </div>                   
                    <div class="searchButtonDiv">
                        <input id = "searchButton" type="button" onclick="fetchFlights()" value="Search Flights" class="searchButton"/>
                    </div>                      
                </div>
                <div class="resultsDiv">   
                    <span style="float:right;" id="resCnt"></span>
                    <table id="flightResults" style="display:none;" class="flightResults">                                                
                    </table>
                </div>
            </div>
            <div id="checkSeats" style="display: none;">
                <div class="searchDiv">
                    <div class="fromSearch">
                        <div class="textBoxHeading">
                            <span>Flight number</span>
                        </div>
                        <div>
                            <input class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all" id="flno">
                        </div>
                    </div>
                    <div class="fromSearch">
                        <div class="textBoxHeading">
                            <span>Date</span>
                        </div>
                        <div>
                            <input class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all" id="dtseat">
                        </div>
                    </div>
                    <div class="searchButtonDiv">
                        <input id = "searchButton2" type="button" onclick="fetchSeatAvailability()" value="Check Seat Availability" class="searchButton"/>
                    </div>
                </div>
                <div id = "seatResults" class="seatRes">

                </div>
            </div>
            <div id="checkFares" style="display: none;">
                <div class="searchDiv">
                    <div class="fromSearch">
                        <div class="textBoxHeading">
                            <span>Flight number</span>
                        </div>
                        <div>
                            <input class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all" id="flnum">
                        </div>
                    </div>                    
                    <div class="searchButtonDiv">
                        <input id = "searchButton3" type="button" onclick="fetchFareDetails()" value="Check Fares" class="searchButton"/>
                    </div>
                </div>
                <div id = "fareResults" class="seatRes">

                </div>
            </div>
            <div id="checkManifest" style="display: none;">                
                    <div class="searchDiv">
                        <div class="fromSearch">
                            <div class="textBoxHeading">
                                <span>Flight number</span>
                            </div>
                            <div>
                                <input class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all" id="flnumber">
                            </div>
                        </div>
                        <div class="fromSearch">
                            <div class="textBoxHeading">
                                <span>Date</span>
                            </div>
                            <div>
                                <input id="custdate" class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all">
                            </div>
                        </div>
                        <div class="searchButtonDiv">
                            <input id = "searchButton4" type="button" onclick="fetchAllCustomers()" value="Check Passenger Manifest" class="searchButton"/>
                        </div>
                    </div>
                    <div id = "custResults" class="seatRes">
                        <table class="fareTable" style="display:none;" id="custTable">
                            <thead><tr><th class="col1">Passenger Name</th><th class="col2">Seat</th><th class="col3">Phone</th></tr></thead>
                            <tbody id="custResultsData"></tbody>
                        </table>
                    </div>                
            </div>
            <div id="searchPassenger" style="display: none;">
                <div class="searchDiv">
                        <div class="fromSearch">
                            <div class="textBoxHeading">
                                <span>Passenger name</span>
                            </div>
                            <div>
                                <input class="custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-all" id="pname">
                            </div>
                        </div>
                        
                        <div class="searchButtonDiv">
                            <input id = "searchButton5" type="button" onclick="fetchAllFlightInst()" value="Search" class="searchButton"/>
                        </div>
                    </div>
                    <div id = "flResults" class="seatRes">
                        <table class="fareTable" style="display:none;" id="flInstTable">
                            <thead><tr><th class="col4">Flight number</th><th class="col5">Date</th><th class="col6">Name</th><th class="col7">(Number of passengers with same name)</th></tr></thead>
                            <tbody id="flInstData"></tbody>
                        </table>
                    </div>  
            </div>
        </div>        
        <script id="flightTableTemplate" type="text/x-jquery-tmpl"> 
            <table class="eachOptionTable">
            {{each flightList}}
            <tr><td class="col1">Flight \${flightNumber}</td><td class="col2"></td><td class="col3"><i>\${weekdays}</i></td></tr>
            <tr><td class="col1">\${departAirportName} (\${departAirportCode})</td><td class="col2">to</td><td class="col3">\${arrivalAirportName}(\${arrivalAirportCode})</td></tr>
            <tr><td class="col1">\${departTime}</td><td class="col2"></td><td class="col3">\${arrivalTime}</td></tr>
            <tr><td class="col1"></td><td class="col2"></td><td class="col3"></td></tr>
            <tr><td class="col1"></td><td class="col2"></td><td class="col3"></td></tr>                        
            {{/each}}                
            </table>
        </script>  
        <script id="seatResTemplate" type="text/x-jquery-tmpl"> 
            <table class="flInstTable">            
            <tr><td class="col1">Flight \${flightNo}</td><td class="col3">Date of journey: \${date}</td></tr>
            <tr><td class="col1">From: \${departName}</td><td class="col3">To: \${arrivalName}</td></tr>
            <tr><td class="col1">Departure time: \${departTime}</td><td class="col3">Arrival time: \${arrivalTime}</td></tr>
            <tr><td class="col1"><b><i>Number of seats available: \${numSeatsAvail}<i></b></td></td><td class="col3"></td></tr>            
            </table>
        </script>  
        <script id="fareResTemplate" type="text/x-jquery-tmpl"> 
            <table class="flInstTable">            
            <tr><td class="col1">Flight \${flightNumber}</td><td class="col3">weekdays: \${weekdays}</td></tr>
            <tr><td class="col1">From: \${departAirportName}(\${departAirportCode})</td><td class="col3">To: \${arrivalAirportName}(\${arrivalAirportCode})</td></tr>
            <tr><td class="col1">Departure time: \${departTime}</td><td class="col3">Arrival time: \${arrivalTime}</td></tr>            
            </table>
            <table class="fareTable">
            <tr><th class="col1">Class</th><th class="col2">Amount(in dollars)</th><th class="col3">Restrictions</th></tr>
            {{each fareDetails}}
            <tr><td class="col1">\${fareCode}</td><td class="col2">\${amount}</td><td class="col3">\${restrictions}</td></tr>                                  
            {{/each}}   
            </table>
        </script>  
        <script id="custResTemplate" type="text/x-jquery-tmpl">                        
            <tr><td class="col1">\${customerName}</td><td class="col2">\${seat}</td><td class="col3">\${customerPhone}</td></tr>                                  
        </script>
        <script id="flInstDataTemplate" type="text/x-jquery-tmpl">                        
            <tr><td class="col4">\${flight}</td><td class="col5">\${date}</td><td class="col6">\${name}</td><td class="col7">(\${count})</td></tr>                                  
        </script>
    </body>
</html>
