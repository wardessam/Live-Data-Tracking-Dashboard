import '../../src/App.css';
import Sidebar from './Sidebar';
import {BrowserRouter as Router,Switch,Route} from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import React, { Component } from 'react';
import {Link} from 'react-router-dom';


class Home extends Component{
  constructor(props){
    super(props);
    this.state = {
      card:[],
      c1:[],
      c2:[],
      c3:[]
    }
  }

  refreshlist() {
    fetch(process.env.REACT_APP_API+'dashbord')
    .then (response => response.json())
    .then(data => {
      this.setState ({card:data});
    });

    fetch(process.env.REACT_APP_API+'chart')
   .then (response => response.json())
   .then(data => {
     this.setState ({c1:data});
   });

   fetch(process.env.REACT_APP_API+'gragh')
   .then (response => response.json())
   .then(data => {
     this.setState ({c2:data});
   });

   fetch(process.env.REACT_APP_API+'itemschart')
   .then (response => response.json())
   .then(data => {
     this.setState ({c3:data});
   });
  }

  componentDidMount(){
    this.refreshlist();
  }

  componentDidUpdate()
  {
    this.refreshlist();
  }

  render(){
    var items = this.state.card.map(it => <div key={it.ID}>{it.Value}</div>);
   
    var i = this.state.c1.map(it => <div key={it.Y}>{it.X}</div>);
    var ii = this.state.c1.map(it => <div key={it.Y}>{it.Y}</div>);

    var ix = this.state.c2.map(it => <div key={it.Y}>{it.X}</div>);

    var iy = this.state.c3.map(it => <div key={it.Y}>{it.X}</div>);
    return (
      <div>
      <div class="container">
        <p></p>
      <div class="card-deck">
      <div class="card bg-danger">
        <div class="card-body text-center">
          <p class="card-text"><strong>Total Sales</strong></p>
          <hr/>
          <p>{items[0]}</p>
        </div>
      </div>
      <div class="card bg-light">
        <div class="card-body text-center">
          <p class="card-text"><strong>Average per Year</strong></p>
          <hr/>
          <p>{items[1]}</p>
        </div>
      </div>
      <div class="card bg-danger">
        <div class="card-body text-center">
          <p class="card-text"><strong>Average per Month</strong></p>
          <hr/>
          <p>{items[2]}</p>
        </div>
      </div>
      <div class="card bg-light">
        <div class="card-body text-center">
          <p class="card-text"><strong>Average per Day</strong></p>
          <hr/>
          <p>{items[3]}</p>
        </div>
      </div>  
    </div>

        <br></br>
        <hr/>

      <div>
      <div class="row">
      <div class="col-lg-6 col-md-6">        
      <div class="card bg-light">
        <div class="card-body text-center">
          <p class="card-text"><strong>Sales Per Week</strong></p>
          <hr/>
          <div class="row justify-content-center">
          <p>{i}</p>
            :-
          <p> <font color="#C80D1B"> {ii} </font></p>
          </div>
        </div>
      </div>
      </div>
      <div class="col-lg-6 col-md-6">
      <div class="card bg-light">
        <div class="card-body text-center">
          <p class="card-text"><strong>Top 10 Branches</strong></p>
          <hr/>
          <div class="row justify-content-center">
          <p>
            {ix}
          </p>
          </div>
        </div>
      </div>
      <p className='branchrank' id='branchrank'> : Is The Branch Rank. </p>
      <label className='branchrank' id='branchrank'> <strong> {items[4]} </strong></label>
      </div>
      </div>
      <br></br>
      <div class="row">
      <div class="col-lg-6 col-md-6">
      <div class="card bg-light">
        <div class="card-body text-center">
        <p class="card-text"><strong>Recommendations</strong></p>
        <hr/>
        <Link to={"/RecommendationList"}><a class="card-link">Normal Recommendations</a></Link>
        <br></br>
        <Link to={"/RecommendationListS"}> <a class="card-link">Seasonal Recommendations</a></Link>
        </div>
        </div>
      </div>
      <div class="col-lg-6 col-md-6">
      <div class="card bg-light">
        <div class="card-body text-center">
          <p class="card-text"><strong>Top 10 Frequnet Item</strong></p>
          <hr/>
          <div class="row justify-content-center">
          <p>
            {iy}
          </p>
          </div>
        </div>
      </div>
      </div>
      </div>
      </div>

      </div>
    </div>
    );
  }
}

export default Home;
