import { notification } from 'antd'
import Title from 'antd/lib/typography/Title'
import {} from 'module'
import React, { useEffect } from 'react'

export function AccessDenied() {
  useEffect(() => {
    notification.error({
      message: 'Access denied',
      description: "You don't have authorized access to this page",
    })
  }, [])

  return (
    <>
      <Title>401 ERROR</Title>
      <h1>Access Denied</h1>
    </>
  )
}
