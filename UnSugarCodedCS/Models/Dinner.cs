using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace UnSugarCodedCS.Models
{
public class Dinner
{



private string _food;
private float _sugar;
private DateTime _stampTime;
private float _carb;

private int _id;
private int _userId;

  public Dinner (string food, DateTime stampTime, float sugar, float carb, int userId, int id=0)
{
	_food = food;
	_stampTime = stampTime;
	_sugar = sugar;
	_carb = carb;
	_userId = userId;
	_id =id;
}

public string GetDinnerFood()
{
	return _food;
}

public float GetDinnerSugar()
{
	return _sugar;
}

public float GetDinnerCarb()
{
	return _carb;
}

public DateTime GetDinnerStampTime()
{
	return _stampTime;
}

public int GetId()
{
	return _id;
}

public int GetUserId()
{
	return _userId;
}


public static List<Dinner> GetAllDinner()
{
	List<Dinner> allDatas = new List<Dinner> {
	};
	MySqlConnection conn = DB.Connection();
	conn.Open();
	MySqlCommand cmd = conn.CreateCommand();
	cmd.CommandText = @"SELECT * FROM dinner;";
	MySqlDataReader rdr = cmd.ExecuteReader();
	while(rdr.Read())
	{
		int dinnerId = rdr.GetInt32(0);
		string food = rdr.GetString(1);
		DateTime stampTimeB = rdr.GetDateTime(2);
		float sugar = rdr.GetFloat(3);
		float carb = rdr.GetFloat(4);
		int user_id = rdr.GetInt32(5);
		Dinner newDinner = new Dinner(food, stampTimeB, sugar, carb, user_id, dinnerId);
		allDatas.Add(newDinner);
	}

	conn.Close();
	if ( conn != null) conn.Dispose();
	return allDatas;
}

public void Save()
{
	MySqlConnection conn =DB.Connection();
	conn.Open();
	MySqlCommand cmd = conn.CreateCommand();
	cmd.CommandText = @"INSERT INTO dinner (food, stampTime, sugar, carb, user_id) VALUES (@food, @stampTime, @sugar, @carb, @user_id);";
	MySqlParameter foodParameter = new MySqlParameter ("@food",this._food);
	cmd.Parameters.Add(foodParameter);
	MySqlParameter stampTimeParameter = new MySqlParameter ("@stampTime",this._stampTime);
	cmd.Parameters.Add(stampTimeParameter);
	MySqlParameter sugarParameter = new MySqlParameter ("@sugar",this._sugar);
	cmd.Parameters.Add(sugarParameter);
	MySqlParameter carbParameter = new MySqlParameter ("@carb",this._carb);
	cmd.Parameters.Add(carbParameter);
	MySqlParameter userParameter = new MySqlParameter ("@user_id",this._userId);
	cmd.Parameters.Add(userParameter);
	cmd.ExecuteNonQuery();
	_id = (int)cmd.LastInsertedId;
	conn.Close();
	if( conn != null) conn.Dispose();
}

public void Delete()
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	MySqlCommand cmd = new MySqlCommand("DELETE FROM dinner WHERE user_id = @SearchId; DELETE FROM dinner WHERE id = @dinnerId;", conn);
	MySqlParameter SearchIdParameter = new MySqlParameter();
	SearchIdParameter.ParameterName = "@SearchId";
	SearchIdParameter.Value = this.GetId();
	cmd.Parameters.Add(SearchIdParameter);
  MySqlParameter dinnerIdParameter = new MySqlParameter();
	dinnerIdParameter.ParameterName = "@dinnerId";
	dinnerIdParameter.Value = this.GetId();
	cmd.Parameters.Add(dinnerIdParameter);
	cmd.ExecuteNonQuery();
	if (conn != null)
	{
		conn.Close();
	}
}


public void Edit(string newFood, DateTime newStampTime, float newSugar, float newCarb)
{
  MySqlConnection conn = DB.Connection();
  conn.Open();
  MySqlCommand cmd = conn.CreateCommand();
  cmd.CommandText = @"UPDATE dinner SET food = @newfood AND stampTime = @newStampTime AND sugar = @newSugar AND carb = @newCarb WHERE id = @searchId;";

  MySqlParameter searchId = new MySqlParameter();
  searchId.ParameterName = "@searchId";
  searchId.Value = _id;
  cmd.Parameters.Add(searchId);

  MySqlParameter food = new MySqlParameter("@newfood",newFood);
  cmd.Parameters.Add(food);
  cmd.ExecuteNonQuery();

  MySqlParameter stampTime = new MySqlParameter("@newStampTime",newStampTime);
  cmd.Parameters.Add(stampTime);
  cmd.ExecuteNonQuery();

  MySqlParameter sugar = new MySqlParameter("@newSugar",newSugar);
  cmd.Parameters.Add(sugar);
  cmd.ExecuteNonQuery();

  MySqlParameter carb = new MySqlParameter("@newCarb",newCarb);
  cmd.Parameters.Add(carb);
  cmd.ExecuteNonQuery();

  _food = newFood;
  _stampTime = newStampTime;
  _sugar = newSugar;
  _carb = newCarb;


  conn.Close();
  if (conn != null)
  {
    conn.Dispose();
  }
}

public static Dinner Find(int id)
{
	MySqlConnection conn = DB.Connection();
	conn.Open();
	var cmd = conn.CreateCommand() as MySqlCommand;
	cmd.CommandText = @"SELECT * FROM dinner WHERE id = (@searchId);";
	MySqlParameter searchId = new MySqlParameter();
	searchId.ParameterName = "@searchId";
	searchId.Value = id;
	cmd.Parameters.Add(searchId);
	var rdr = cmd.ExecuteReader() as MySqlDataReader;
	int DinnerId = 0;
	string food = "";
	DateTime stampTime = new DateTime();
	float sugar = 0;
	float carb = 0;
  int userId = 0;
	while(rdr.Read())
	{
		DinnerId = rdr.GetInt32(0);
		food = rdr.GetString(1);
		stampTime = rdr.GetDateTime(2);
		sugar = rdr.GetFloat(3);
		carb = rdr.GetFloat(4);
    userId = rdr.GetInt32(5);
	}
	Dinner newDinner = new Dinner(food, stampTime, sugar, carb, userId, DinnerId);
	conn.Close();
	if (conn != null)
	{
		conn.Dispose();
	}
	return newDinner;
}

}
}
