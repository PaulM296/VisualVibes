import React from "react";
import Home from "./Pages/Home/Home";

export type userContextType = {
  userName: string;
  setUserName: (value: string) => void;
}

export const userContext = React.createContext({} as userContextType)

const App = () => {

  return (
    <div>
      <Home />
    </div>
  )
}

export default App
