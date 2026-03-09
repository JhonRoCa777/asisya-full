import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Provider } from 'react-redux';
import { Store } from './store';
import { AuthGuard } from './guards/Auth';
import { SuspenseLazy } from './components/SuspenseLazy';
import { ROUTER } from './router';
import LoginPage from './pages/LoginPage';

function App() {

  return (
    <Provider store={Store}>
      <BrowserRouter>

        <Routes>

          <Route path={ROUTER.MAIN} element={<LoginPage/>}/>

          <Route element={<AuthGuard/>}>
            <Route path={`${ROUTER.HOME.MAIN}/*`} element={<SuspenseLazy path={import('./pages/Home')}/>}/>
          </Route>
          
        </Routes>
      </BrowserRouter>
    </Provider>
  )
}

export default App
