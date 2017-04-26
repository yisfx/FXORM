# FXORM
.Net Core FXORM
ORM<User> orm=new ORM<User>();
var result=orm.Eq("ID",1).Lg("Age",18).Select();
var user=new User(){ID=2,Age=18};
orm.Add(user);
user.Age=13;
orm.Modetify(user);
orm.Delete(user);
