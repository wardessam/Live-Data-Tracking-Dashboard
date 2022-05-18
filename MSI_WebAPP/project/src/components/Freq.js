import React, { Component, Fragment } from 'react';
import './Report.css';
import {RiFileExcel2Line} from "react-icons/ri";
import {AiOutlineFilePdf} from "react-icons/ai";
import {BrowserRouter as Router,Switch,Route} from 'react-router-dom';
import jsPDF from "jspdf";
import "jspdf-autotable";
import { render } from '@testing-library/react';
import ReactHTMLTableToExcel from 'react-html-table-to-excel';

class Report extends Component {

    constructor(props){
        super(props);
        this.state={
            reps:[]
        }
    }

    refreshlist() {
        fetch(process.env.REACT_APP_API+'Freq')
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

    render(){
        const{reps}=this.state;
        return (
            <div>
                <div >
                    <h1 className="headingText"><strong>Daliy FI Report</strong></h1>
                </div>

                <div className="row justify-content-center table-wrapper-scroll-y my-custom-scrollbar ps ps--active-y">
                    <div className="col-md-12" id="thetable">
                        <div className="table-responsive-md table-striped table-bordered table-hover">    
                        <ReactHTMLTableToExcel
                            id="test-table-xls-button"
                            className="download-table-xls-button"
                            table="table-to-xls"
                            filename="tablexls"
                            sheet="tablexls"
                            buttonText="Download as XLS"/>  
                            <table className="table" id="table-to-xls">
                                <thead>
                                    <tr>
                                        <th>Frequent Items</th>
                                        <th>Amount</th>
                                        <th>Date</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {reps.map(rep=>
                                        <tr key ={rep.Freq} >
                                            <td>{rep.Freq}</td>
                                            <td>{rep.Amount}</td>
                                            <td>{rep.Date}</td>
                                        </tr>
                                        )}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                

            </div>
                
        );
    }
}

export default Report;