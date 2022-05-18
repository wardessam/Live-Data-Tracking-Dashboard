import React from 'react';
import Login from './components/Login';
import Register from './components/Register';

import './App.css';
import Sidebar from './components/Sidebar';
import Home from './components/Home';
import UpdateData from './components/UpdateData';
import Routes from './components/routes';
import {BrowserRouter as Router, Route, Switch,Redirect} from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';

function App() {
  return (
    <Router>
      <Switch>
    <Route exact path = "/" component={Login}/>
    <Route path = "/Register" component={Register}/>
    <Route path="/Home" component={Routes}/>
    <Route path = "/UpdateData" component={UpdateData}/>
   </Switch>
  </Router>
  );
}

export default App;
