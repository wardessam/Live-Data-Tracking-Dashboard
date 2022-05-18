import React, {Component} from 'react';
import './Setting.css'
import Sidebar from './Sidebar';
import {BrowserRouter as Router,Switch,Route,Link} from 'react-router-dom';

class Setting  extends Component {

    constructor() {
        super();
        this.state = {
            Password: '',
        }
        this.Password = this.Password.bind(this);
        this.login = this.login.bind(this);
    }

    Password(event) {
        this.setState({ Password: event.target.value })
    }

    login(event){
        event.preventDefault();
        if(this.state.Password === "msi2021")
        {
            const currentURL = window.location.href
            console.log(currentURL);
            const pathname = window.location.pathname;
            console.log(pathname);
            console.log("entereeed");
            window.location.pathname="/UpdateData";
        }else
        {
            alert("Invalid Admin Password");
        }    
    }
    render(){
        return(
            <div>
                <div className="innercontent" >
                    <form>
                    <h3 className="HeadText">Admin Password</h3>
                    <input type="password" onChange={this.Password} className="InputPassword" id="exampleInputPassword1" placeholder="Password"/>
                    <button type="submit" onClick={this.login} className="btn btn-primary">Submit</button>
                    </form>
                </div>
            </div>
        )
    }

}

export default Setting;