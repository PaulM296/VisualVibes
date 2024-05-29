import Feed from '../../Components/Feed/Feed'
import Navbar from '../../Components/Navbar/Navbar'
import Rightbar from '../../Components/Rightbar/Rightbar'
import Sidebar from '../../Components/Sidebar/Sidebar'
import "./Home.css"

const Home = () => (
  <div className="homeContainer">
    <Navbar />
    <div className="homepageContent">
      <Sidebar />
      <Feed />
      <Rightbar />
    </div>
  </div>
)

export default Home