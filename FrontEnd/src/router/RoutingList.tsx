import React from 'react';
import { Route } from 'react-router-dom';
import { CreateAssignment } from '../pages/assignments/Create';
import { ListAssignments } from '../pages/assignments/List';
import { ListAssignmentsForEachUser } from '../pages/assignments/ListByUser';
import { UpdateAssignment } from '../pages/assignments/Update';
import { CreateAsset } from '../pages/assets/Create';
import { ListAssets } from '../pages/assets/List';
import { UpdateAsset } from '../pages/assets/Update';
import { AccessDenied } from '../pages/errors/AccessDenied';
import { NotFound } from '../pages/errors/NotFound';
import { Login } from '../pages/login/Login';
import { ListReturnRequests } from '../pages/return-requests/ReturnList';
import { ReportView } from '../pages/report/Report';
import { CreateUser } from '../pages/users/Create';
import { ListUsers } from '../pages/users/List';
import { UpdateUser } from '../pages/users/Update';

const routes = [
  {
    path: '/400-not-found',
    component: NotFound,
    key: '/400-not-found',
  },
  {
    path: '/401-access-denied',
    component: AccessDenied,
    key: '/401-access-denied',
  },
  {
    path: '/login',
    component: Login,
    key: '/login',
  },
  {
    path: '/users',
    component: ListUsers,
    key: '/users'
  },
  {
    path: '/users/update/:userId',
    component: UpdateUser,
    key: '/users/update/:userId',
  },
  {
    path: '/users/create',
    component: CreateUser,
    key: '/users/create',
  },
  {
    path: '/assignments',
    component: ListAssignments,
    key: '/assignments',
  },
  {
    path: '/home',
    component: ListAssignmentsForEachUser,
    key: '/home',
  },
  {
    path: '/assignments/create',
    component: CreateAssignment,
    key: '/assignments/create',
  },
  {
    path: '/assignments/update/:assignmentId',
    component: UpdateAssignment,
    key: '/assignments/update/:assignmentId',
  },
  {
    path: '/return-requests',
    component: ListReturnRequests,
    key: '/return-requests',
  },
  {
    path: '/assets/create',
    component: CreateAsset,
    key: '/assets/create',
  },
  {
    path: '/assets',
    component: ListAssets,
    key: '/assets',
  },
  {
    path: '/assets/update/:assetId',
    component: UpdateAsset,
    key: '/assets/update/:assetId',
  },
  {
    path: "/reports",
    component: ReportView,
    key: "/reports",
  },
];

export function RoutingList(): JSX.Element {
  return <>
    {
      routes.map(item => {
        if (item.path.split('/').length === 2) {
          return (
            <Route
              exact
              path={item.path}
              component={item.component}
              key={item.key}
            />
          );
        }
        return <Route path={item.path} component={item.component} key={item.key} />;
      })
    }
  </>
}
