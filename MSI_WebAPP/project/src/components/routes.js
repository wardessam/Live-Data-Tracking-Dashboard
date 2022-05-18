import React from "react";
import {BrowserRouter, Route, Switch} from "react-router-dom";
import Sidebar from './Sidebar';
import Home from "./Home";
import Report from './Report';
import Reportm from './Reportm';
import Reporty from './Reporty';
import Reportq from './Reportq';
import Freq from './Freq';
import Freqm from './Freqm';
import Freqy from './Freqy';
import Freqq from './Freqq';
import Setting from './Setting';
import UpdateData from './UpdateData';
import RecommendationList from './RecommendationList';
import RecommendationListS from './RecommendationListS';
function Routes() {
    return (
        <BrowserRouter>
        <Sidebar/>
            <Switch>
                <Route path="/Home" exact component={Home}/>
                <Route path = "/UpdateData" component={UpdateData}/>
                <Route path = "/Sreports/Daily_Sales" component={Report}/>
                <Route path = "/Sreports/Monthly_Sales" component={Reportm}/>
               <Route path = "/Sreports/Yearly_Sales" component={Reporty}/>
               <Route path = "/Sreports/Quarterly_Sales" component={Reportq}/>
               <Route path = "/Freports/FI_Daily" component={Freq}/>
            <Route path = "/Freports/FI_Monthly" component={Freqm}/>
            <Route path = "/Freports/FI_Yearly" component={Freqy}/>
            <Route path = "/Freports/FI_Quarterly" component={Freqq}/>
            <Route path = "/Setting" component={Setting}/>
            <Route path = "/RecommendationList" component={RecommendationList}/>
            <Route path = "/RecommendationListS" component={RecommendationListS}/>
            </Switch>
        </BrowserRouter>
    )
}

export default Routes;