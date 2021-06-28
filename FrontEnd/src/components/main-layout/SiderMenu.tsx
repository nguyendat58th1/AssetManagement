import React from 'react'
import { Layout, Menu } from 'antd'
import {
  LoginOutlined,
  UserOutlined,
  RollbackOutlined,
  DollarOutlined,
  SolutionOutlined,
  HomeOutlined,
  CarryOutOutlined
} from "@ant-design/icons";
import { UserType } from "../../models/UserModel";
import { Link, useLocation } from "react-router-dom";

const { Sider } = Layout

function SiderMenu() {
  let location = useLocation()
  let userType = sessionStorage.getItem('type')

  return (
    <Sider
      width={200}
      className="site-layout-background"
      style={{
        marginTop: 50,
        overflow: "auto",
        height: "100vh",
        position: "fixed",
        left: 0,
        boxShadow: '0 12px 16px 0 rgba(0, 0, 0, 0.24), 0 17px 50px 0 rgba(75, 73, 73, 0.19)',
      }}
    >
      <Menu
        theme="light"
        mode="inline"
        style={{ height: "100%", borderRight: 0 }}
        selectedKeys={[location.pathname]}
      >
        {userType && (
          <Menu.Item
            icon={<HomeOutlined />}
            key="/home"
            className="menu-item"
          >
            <Link to="/home">Home</Link>
          </Menu.Item>
        )}
        {userType && userType === UserType[UserType.Admin] && (
          <>
            <Menu.Item
              icon={<UserOutlined />}
              key="/users"
              className="menu-item"
            >
              <Link to="/users">Users</Link>
            </Menu.Item>
            <Menu.Item
              icon={<DollarOutlined />}
              key="/assets"
              className="menu-item"
            >
              <Link to="/assets">Assets</Link>
            </Menu.Item>
            <Menu.Item
              icon={<SolutionOutlined />}
              key="/assignments"
              className="menu-item"
            >
              <Link to="/assignments">Assignments</Link>
            </Menu.Item>
            <Menu.Item
              icon={<RollbackOutlined />}
              key="/return-requests"
              className="menu-item"
            >
              <Link to="/return-requests">Return Requests</Link>
            </Menu.Item>
            <Menu.Item
              icon={<CarryOutOutlined />}
              key="/reports"
              className="menu-item">
              <Link to="/reports">Report</Link>
            </Menu.Item>
          </>
        )}
        {userType && userType === UserType[UserType.User].toUpperCase() && (
          <>
            {/* <Menu.Item
              icon={<UserOutlined />}
              key="/users"
              className="menu-item"
            >
              <Link to="/users">Users</Link>
            </Menu.Item> */}
          </>
        )}
        {!userType && (
          <Menu.Item
            icon={<LoginOutlined />}
            key="/login"
            className="menu-item"
          >
            <Link to="/login">Login</Link>
          </Menu.Item>
        )}
      </Menu>
    </Sider>
  )
}

export default SiderMenu
