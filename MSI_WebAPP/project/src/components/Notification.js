import React , {Component}from 'react';
import styled from 'styled-components';
import './Notification.css';



    /**
     * 
     * import React , {Component}from 'react'
import NotificationsNoneIcon from '@material-ui/icons/NotificationsNone';
import Badge from '@material-ui/core/Badge';
import IconButton from '@material-ui/core/IconButton';
     * 
     * 
     * 
     *   invisible :   "false" will hide padge , "true" will show padge
     *   
     * 
                <Badge badgeContent={this.state.NotificationData.length} color="error">
                    <NotificationsNoneIcon />
                </Badge>



                
class Notification extends Component{

    
    state = {
        BadgeCount : 3 ,

        NotificationData : [
            {notificationheader : "Dashboard has been updated" , time : 12344555} ,
            {notificationheader : "Reports has been updated" , time : 12344555} ,
        ]
    }


    handlePlus = () => {
        const PrevBadgeCount = this.state.BadgeCount
        this.setState({
            BadgeCount : PrevBadgeCount+1,
        })
    }

    handleMinus = () => {
        const PrevBadgeCount = this.state.BadgeCount
        if(PrevBadgeCount === 0){
            console.log("Can not Minus ya Gimmy")
            return
        }
        this.setState({
            BadgeCount : PrevBadgeCount-1,
            BadgeVisibility : "true"
        })
        
    }
    render(){
        
        return(
            <div>
                <div className="dropdown">
                    <IconButton aria-label="notification" color="inherit" data-toggle="dropdown">
                        <Badge badgeContent={this.state.NotificationData.length} color="error">
                            <NotificationsNoneIcon />
                        </Badge>
                    </IconButton>
                    <ul className="dropdown-menu" aria-labelledby="dropdownMenu2">
                        <li><button className="dropdown-item" type="button">Action</button></li>
                        <li><button className="dropdown-item" type="button">Another action</button></li>
                        <li><button className="dropdown-item" type="button">Something else here</button></li>
                    </ul>
                </div>
            </div>
        );
    }

}

export default Notification

     */
const NotifyButton=styled.span`
    color:white;

`;

class Notification extends Component{

    
    state = {
        notificationData : [
            {notificationContent : "Dashboard has been Updated" , notificationTime : "2 hours ago" , status : "new"},
            
            {notificationContent : "Reports has been Updated" , notificationTime : "5 days ago" , status : "new"},
        ]

        , 

        numberofseenNotifications : 0
    }


    handleNotificationClick = (indexOfClickednotification) => {
        
        /*console.log(indexOfClickednotification)
        const filteredArray = this.state.notificationData.filter((_, i) => i !== indexOfClickednotification);
        this.setState({
            notificationData: filteredArray
        });*/


        let currentClickednotification = this.state.notificationData[indexOfClickednotification];
        if(currentClickednotification.status === "new"){
            currentClickednotification.status = "old"
            
            let notificationDataBeforeEdit = this.state.notificationData;
            let numberofseenNotificationsBeforeEdit = this.state.numberofseenNotifications + 1;

            notificationDataBeforeEdit[indexOfClickednotification] = currentClickednotification;
            this.setState({
                notificationData : notificationDataBeforeEdit,
                numberofseenNotifications : numberofseenNotificationsBeforeEdit
            })
        }

    }

    render(){
        
        return(
            <div>
                <div>
                    <div className="dropdown">
                    <button type="button" className="icon-button"  data-toggle="dropdown">
                        <NotifyButton>
                         <span className="material-icons">notifications</span>
                        </NotifyButton>
                        {
                            this.state.notificationData.length - this.state.numberofseenNotifications > 0 ? <span className="icon-button__badge">{this.state.notificationData.length - this.state.numberofseenNotifications}</span> : null 
                        }
                    </button>
                    

                    <ul className="dropdown-menu">
                        <li>
                            <hr></hr>
                        </li> 
                        {
                            this.state.notificationData.map( (item , index) =>{
                                
                                return(
                                   <React.Fragment key={index+1}>
                                        <li>
                                            <hr></hr>
                                        </li>
                                        <li >
                                            <a onClick={() => this.handleNotificationClick(index)}>
                                                {
                                                    item.status === "new" ? 
                                                    
                                                        <>
                                                            <div className="NotificationName">{item.notificationContent}</div>
                                                            <div className="NotificationTime NewNotification">{item.notificationTime}</div>
                                                        </>    
                                                        
                                                     : 
                                                        <>
                                                            <div className="NotificationName">{item.notificationContent}</div>
                                                            <div className="NotificationTime">{item.notificationTime}</div>
                                                        </> 
                                                    
                                                }
                                            </a>
                                        </li>
                                   </React.Fragment>
                                )
                            })
                        }
                        <li>
                            <hr></hr>
                        </li>
                    </ul>
                    </div>
                </div>
                
            </div>
        );
    }

}

export default Notification