import React, { useState } from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import * as FaIcons from 'react-icons/fa';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import { SidebarData } from './SidebarData';
import SubMenu from './SubMenu';
import { IconContext } from 'react-icons/lib';
import logo from './Logo.PNG';
import { RiGitBranchFill } from 'react-icons/ri';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Dropdown } from 'bootstrap';
import Notification from './Notification';

const Nav = styled.div`
  background: #00427E  ;
  height: 50%;
  width:100%;
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;

const NavIcon = styled(Link)`
  margin-left: 3rem;
  
  font-size: 2rem;
  height: 80px;
  display: flex;
  justify-content: flex-start;
  align-items: center;
`;

const SidebarNav = styled.nav`
  background: #00427E;
  width: 40%;
  height: 100vh;
  display: flex;
  color: #fff;
  justify-content: center;
  position: fixed;
  top: 0;
  left: ${({ sidebar }) => (sidebar ? '0' : '-100%')};
  transition: 350ms;
  z-index: 10;
`;

const SidebarWrap = styled.div`
  width: 100%;
`;

const Notify=styled.div`
  margin-right: 2rem;
  font-size: 2rem;
  height: 80px;
  display: flex;
  align-items: center;
  float:left;
`;

const LogOut =styled.div`
margin-right: 2rem;
font-size: 1.5rem;
background: #00427E;
height: 80px;
display: flex;
align-items: center;
float:right;
padding-left: 10px;
`;

const MyDrop =styled.div`
  padding-left: 50px;
  width:70%
`;



const Sidebar = () => {
  const [sidebar, setSidebar] = useState(false);
  const showSidebar = () => setSidebar(!sidebar);
  const showNotification = () => {};
  const logout = () =>{};
  return (
    <>
      <IconContext.Provider value={{ color: '#fff' }}>
        <Nav>
          
          <NavIcon to='#'>
            <FaIcons.FaBars onClick={showSidebar} />
          </NavIcon>
          <MyDrop className="dropdown"> 
          </MyDrop>
          <Notify>
            <Notification/>
          </Notify>
          <LogOut> 
            <a href="/">
              <AiIcons.AiOutlineLogout/>
            </a> 
          </LogOut>
        </Nav>
        <SidebarNav sidebar={sidebar}>
          <SidebarWrap>
              <img width={'100%'} height={'13%'} src={logo}/>
            <NavIcon to='#'>
                <AiIcons.AiOutlineClose onClick={showSidebar} />
            </NavIcon>
            {SidebarData.map((item, index) => {
              return <SubMenu item={item} key={index} />;
            })}
          </SidebarWrap>
        </SidebarNav>
      </IconContext.Provider>
    </>
  );
};

export default Sidebar;
