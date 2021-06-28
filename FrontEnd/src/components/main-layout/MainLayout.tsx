import { Layout } from 'antd';
import React from 'react';
import { RoutingList } from '../../router/RoutingList';
import LayoutBanner from './LayoutBanner';
import SiderMenu from './SiderMenu';

const { Content } = Layout;

function MainLayout() {
  return (
    <Layout>
      <SiderMenu />
      <Layout>
        <LayoutBanner />
        <Content className="site-layout" style={{ margin: '30px 16px 0px 200px' }}>
          <div style={{ padding: 24, background: '#fff', minHeight: 20 }}>
            <RoutingList />
          </div>
        </Content>
      </Layout>
    </Layout>
  );
}

export default MainLayout;
