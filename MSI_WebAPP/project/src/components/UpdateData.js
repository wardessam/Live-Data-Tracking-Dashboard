import React, {Component} from 'react';
import loginimg from '../../src/Update.png';
import logo from '../../src/LogoName.jpg'
import {Link} from 'react-router-dom';
import axios from 'axios';
class UpdateData extends Component{
  constructor(){
    super();
     this.state = {
            First_Name:'',
            Last_Name:'',
            User_Name:'',
            Password:'',
            confirm_password:'',
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
            firstName:this.state.First_Name,
            lastName:this.state.Last_Name,
            Email:this.state.User_Name,
            password:this.state.Password,
            confirm_password:this.state.confirm_password};
            let errors = {};
            let isValid = true;
        
            if (!input["firstName"]) {
              isValid = false;
              errors["firstName"] = "Please enter your first name.";
            }
            if (!input["lastName"]) {
                isValid = false;
                errors["lastName"] = "Please enter your last name.";
              }
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
        
            if (!input["confirm_password"]) {
              isValid = false;
              errors["confirm_password"] = "Please confirm your password.";
            }
        
            if (typeof input["password"] !== "undefined" && typeof input["confirm_password"] !== "undefined") {
                
              if (input["password"] != input["confirm_password"]) {
                isValid = false;
                errors["confirm_password"] = "Passwords don't match.";
              }
            } 
        
            this.setState({
              errors: errors
            });
        
            return isValid;
        }

componentWillMount(){
       axios.get("http://localhost:5000/api/userdata")
        .then(res=>{
            if(res.status===200){
              let u = res.data
              this.setState({
              First_Name : u[0].First,
              Last_Name: u[0].Last,
              User_Name:u[0].UserName,
              Password : u[0].Pass,
              confirm_password :u[0].Pass,
              Branch_ID:u[0].BID});
                
               
            }
            else{
                console.log(res.status)
            }
           
        }).catch((err) => alert(err.message))
    }

 handleSubmit= event=>{
        event.preventDefault();
        if(this.validate()){
       console.log("Entered!!");
       const user ={
        First_Name:this.state.First_Name,
        Last_Name:this.state.Last_Name,
        User_Name:this.state.User_Name,
        Password:this.state.Password,
        Branch_ID:this.state.Branch_ID
        };
        axios.put("http://localhost:5000/api/User",user)
        .then(res=>{
            if(res.status===200){
              alert('Updated Successfully');
                window.location = "/Home";
                
               
            }
            else{
               alert('Error in Updating');
                console.log(res.status)
            }
           
        }).catch((err) => alert(err.message))
       
        
       
        let input = {};
        input["firstName"] = "";
        input["lastName"] = "";
        input["Email"] = "";
        input["password"] = "";
        input["confirm_password"] = "";
        this.setState({input:input});
  
       
    }
    };

 render(){
   
     return(
         <div>
          <div className="rightHalfUpdate">
          <form className="formLogin" onSubmit={this.handleSubmit}>
              
              
              <h1 className="headForLogin">Set Up Your Data</h1>
              
              <div className="form-row">
              <div className="form-group col-md-6">
              <label htmlFor="firstName">First Name</label>
               <input type="text" className="form-control" id="firstName" placeholder="" onChange={(e)=>this.setState({First_Name:e.target.value})} value={this.state.First_Name}/>
             <div className="text-danger">{this.state.errors.firstName}  </div>
              </div>
              <div className="form-group col-md-6">
               <label htmlFor="lastNames">Last Name</label>
               <input type="text" className="form-control" id="lastName" placeholder="" onChange={(e)=>this.setState({Last_Name:e.target.value})} value={this.state.Last_Name}/>
                <div  className="text-danger">{this.state.errors.lastName }</div>
               </div>
              </div>



           <div className="form-group">
            <label htmlFor="exampleInputEmail1">Email Address</label>
            <input type="email" className="form-control" id="Email" aria-describedby="emailHelp" placeholder="Enter Email" onChange={(e)=>this.setState({User_Name:e.target.value})} value={this.state.User_Name} />
            <small id="emailHelp" className="form-text text-muted">We'll never share your email with anyone else.</small>
            <div className="text-danger">{this.state.errors.Email}</div>
           </div>



           <div className="form-row">
              <div className="form-group col-md-6">
              <label htmlFor="exampleInputPassword1">Password</label>
            <input type="password" className="form-control" id="password" placeholder="" onChange={(e)=>this.setState({Password:e.target.value})} value={this.state.Password}/>
            <small id="passwordHelpBlock" className="form-text text-muted">
              Your password must be 8-20 characters long, contain letters and numbers, and must not contain spaces or special characters.
             </small>
             <div className="text-danger">{this.state.errors.password}</div>
              </div>
            <div className="form-group col-md-6">
              <label htmlFor="exampleInputPassword1">Confirm Password</label>
            <input type="password" className="form-control" id="confirm_password" placeholder="Confirm Password" onChange={(e)=>this.setState({confirm_password:e.target.value})} value={this.state.confirm_password}/>
                 <div className="text-danger">{this.state.errors.confirm_password}</div>
               </div>
               
              </div>

              <label className="my-1 mr-2" htmlFor="inlineFormCustomSelectBranch">Branch ID</label>
              <label className="my-1 mr-sm-2" htmlFor="inlineFormCustomSelectBranch"> <strong>{this.state.Branch_ID}</strong></label>
            
           
           <div>
           <Link to={"/Home"}><button type="submit" onClick={this.handleSubmit} className="btn btn-primary">Update</button></Link>
           </div>
           </form>
           
          </div>
         </div>
     )
 }

}
export default UpdateData;