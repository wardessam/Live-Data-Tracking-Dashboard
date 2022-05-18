import React , {Component, Fragment} from 'react';
import './RecommendationList.css'
class RecommendationList extends Component{
  
  constructor(props){
    super(props);
    this.state={
        reps:[]
    }
}

refreshlist() {
    fetch(process.env.REACT_APP_API+'Recs')
    .then (response => response.json())
    .then(data => {
    this.setState ({reps:data});
});
}

componentDidMount(){
    this.refreshlist();
}

componentDidUpdate()
{
    this.refreshlist();
}

    RecommendationHeader = "Seasonal Recommendations"

    
    render(){
      const{reps}=this.state;
      return (
        <div>
           
            <div className="RecommendationHeader">
              <h2><strong>{this.RecommendationHeader}</strong></h2>
            </div>
                
            <ol className="RecommendationList">
                  
                {reps.map(rep=>
              <Fragment key ={rep.Freq} >
                  <li className="even" >The item(s) {rep.Freq} are recommended for season {rep.Seas} with 
                  average amaount {rep.Amount} </li>
              </Fragment>
              )}

                          
            </ol>
        </div>
      );
    }
}


export default RecommendationList;
