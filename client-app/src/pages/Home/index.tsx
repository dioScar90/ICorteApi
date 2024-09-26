import { Link } from "react-router-dom";

export function Home() {
  return (
    <>
      <p>Hello! This is my Home Page.</p>
      <Link to={"/login"}>Login</Link>
    </>
  )
}
