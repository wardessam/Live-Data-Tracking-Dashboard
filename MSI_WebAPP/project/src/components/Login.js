import React, {Component} from 'react';
import loginimg from '../../src/login.png';
import logo from '../../src/LogoName.jpg'
import {Link} from 'react-router-dom';
import axios from 'axios';
class Login extends Component{
    constructor(){
        super();
        this.state = {
            First_Name:'',
            Last_Name:'',
            User_Name:'',
            Password:'',
            Branch_ID:0,
            Level:1,
            input: {},
            errors: {}
        }
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);

    }
    handleChange(event) {
        let input = this.state.input;
        input[event.target.name] = event.target.value;
      
        this.setState({
          input
        });
      }

      validate(){
        let input = {
        Email:this.state.User_Name,
        password:this.state.Password};
        let errors = {};
        let isValid = true;
    
    
        if (!input["Email"]) {
          isValid = false;
          errors["Email"] = "Please enter your email Address.";
        }
    
        if (typeof input["Email"] !== "undefined") {
            
          var pattern = new RegExp(/^(("[\w-\s]+")|([\w-]+(?:\.[\w-]+)*)|("[\w-\s]+")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)/i);
          if (!pattern.test(input["Email"])) {
            isValid = false;
            errors["Email"] = "Please enter valid email address.";
          }
        }
    
        if (!input["password"]) {
          isValid = false;
          errors["password"] = "Please enter your password.";
        }
    
        this.setState({
          errors: errors
        });
    
        return isValid;
    }

    handleSubmit= event=>{
        event.preventDefault();
       if(this.validate()){
       console.log("Entered!!");
       console.log(this.state.User_Name);
       console.log(this.state.Password);
       console.log(this.state.Level);
       let e = this.state.User_Name;
       let p = this.state.Password;
        const user = JSON.stringify({
                Email: this.state.User_Name,
                Password: this.state.Password
            });
        console.log(user);
        
        axios.post("http://localhost:5000/api/Login",user,{headers:{ 
          'Accept': 'application/json',
          'Content-Type': 'application/json'}})
        .then(res=>{
          console.log(res);
            if(res.status===200){
              if(res.data.Status==="Success"){
                console.log(res);
                console.log(res.data);
                const result = res.data;
                this.setState({
                    First_Name:result.First_Name,
                    Last_Name:result.Last_Name,
                    User_Name:result.User_Name,
                    Password:result.Password,
                    Branch_ID:result.Branch_ID,
                    Level:result.Level
                    
                })
                console.log("Successsssssssss");
                 alert('Signed In Successfully');
                window.location = "/Home";
               
            }
            else
            {
                alert('Invalid UserName or Password');
            }
            }
            else{
                console.log(res.status)
            }
           
        }).catch((err) => alert(err.message))
        let input = {};
       
        input["Email"] = "";
        input["password"] = "";
       
        this.setState({input:input});
  
       
    }
    };
    

   
 render(){
     
     return(
         <div>
          <div>
              <img src={loginimg} className="leftHalf"/>
              <img src={logo} className="image2"/>
             
          </div>
          <div className="rightHalf">
          <form className="formLogin">
              <img src={logo} className="hiddenlogo"/>
              <h1 className="headForLogin">Login</h1>
           <div className="form-group">
            <label htmlFor="exampleInputEmail1">Email address</label>
            <input type="email" className="form-control" id="Email" aria-describedby="emailHelp" placeholder="Enter email"  onChange={(e)=>this.setState({User_Name:e.target.value})}/>
            <small id="emailHelp" className="form-text text-muted">We'll never share your email with anyone else.</small>
            <div className="text-danger">{this.state.errors.Email}</div>
           </div>
            <div className="form-group">
            <label htmlFor="exampleInputPassword1">Password</label>
            <input type="password" className="form-control" id="Password" placeholder="Password" onChange={(e)=>this.setState({Password:e.target.value})}/>
            <div className="text-danger">{this.state.errors.password}</div>
            </div>
            <div className="row">
            <div className="col">
            <div className="form-check">
              </div>
              </div>
              <div className="col">
              <label>{this.state.userData}</label>
              </div>
           </div>
           <div>
           <Link to={"/Home"}><button type="submit" onClick={this.handleSubmit} className="btn btn-primary">Login</button></Link>
           </div>
           <label className="letterMargin">Don't have an account? <Link to={"/Register"}><label className="registerLetter">Register</label></Link></label>
           </form>
           
          </div>
         </div>
     )
 }

}
export default Login;