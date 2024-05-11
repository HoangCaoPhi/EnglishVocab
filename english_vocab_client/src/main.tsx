import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider } from 'react-router-dom'
import router from './routers/index.tsx'
import { StyledEngineProvider } from '@mui/material'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
      <StyledEngineProvider injectFirst>
        <RouterProvider router={router}></RouterProvider>
      </StyledEngineProvider>
  </React.StrictMode>
)
