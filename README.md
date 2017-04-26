# FXORM<br />
.Net Core FXORM<br />
ORM<User> orm=new ORM<User>();<br />
var result=orm.Eq("ID",1).Lg("Age",18).Select();<br />
var user=new User(){ID=2,Age=18};<br />
orm.Add(user);<br />
user.Age=13;<br />
orm.Modetify(user);<br />
orm.Delete(user);<br />
