import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import { MainProviders } from './components/MainProviders'
import { App } from './App'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <MainProviders>
      <App />
    </MainProviders>
  </StrictMode>,
)
