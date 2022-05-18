import React from 'react';
import * as FaIcons from 'react-icons/fi';
import * as AiIcons from 'react-icons/ai';
import * as IoIcons from 'react-icons/io';
import * as RiIcons from 'react-icons/ri';


export const SidebarData = [
  {
    title: 'Home',
    path: '/Home',
    icon: <AiIcons.AiFillHome />,
  },
  {
    title: 'Sales Reports',
    path: '/Home',
    icon: <IoIcons.IoIosPaper />,
    iconClosed: <RiIcons.RiArrowDownSFill />,
    iconOpened: <RiIcons.RiArrowUpSFill />,

    subNav: [
      {
        title: 'Daily Sales',
        path: '/Sreports/Daily_Sales',
        icon: <IoIcons.IoIosPaper />,
        cName: 'sub-nav'
      },
      {
        title: 'Monthly Sales',
        path: '/Sreports/Monthly_Sales',
        icon: <IoIcons.IoIosPaper />,
        cName: 'sub-nav'
      },
      {
        title: 'Yearly Sales',
        path: '/Sreports/Yearly_Sales',
        icon: <IoIcons.IoIosPaper />
      },
      {
        title: 'Quarterly Sales',
        path: '/Sreports/Quarterly_Sales',
        icon: <IoIcons.IoIosPaper />
      }
    ]
  },
  {
    title: 'Frequent Item Reports',
    path: '/Home',
    icon: <IoIcons.IoIosPaper />,
    iconClosed: <RiIcons.RiArrowDownSFill />,
    iconOpened: <RiIcons.RiArrowUpSFill />,

    subNav: [
      {
        title: 'FI Daily',
        path: '/Freports/FI_Daily',
        icon: <IoIcons.IoIosPaper />,
        cName: 'sub-nav'
      },
      {
        title: 'FI Monthly',
        path: '/Freports/FI_Monthly',
        icon: <IoIcons.IoIosPaper />,
        cName: 'sub-nav'
      },
      {
        title: 'FI Yearly',
        path: '/Freports/FI_Yearly',
        icon: <IoIcons.IoIosPaper />
      },
      {
        title: 'FI Quarterly',
        path: '/Freports/FI_Quarterly',
        icon: <IoIcons.IoIosPaper />
      }
    ]
  },
  {
    title: 'Setting',
    path: '/Setting',
    icon: <AiIcons.AiFillSetting />,
  }
];
