//===== rAthena Script =======================================
//= saders Hourly Rewards
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/ (this the first version there is no url yet)
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== all the configuration are in the last
//==== support gepard / ip / or without them
//==== if you use ip/gepard sql will be used for that
//==== can add minimem level to get the rewards
//==== can change the time
//==== can ban the vending from the rewards (it will reset after relogin)
//==== can add rewards else then variable
//==== there is a shop npc for the variable
//==== you can make it only for vip
//==== can ban idle players for X time from the Hourly Rewards
//==== player can ban his char from getting the reward by @HourlyBan (it will reset after relogin)
//============================================================
//==== please send me a message if you find error
//==== if you like my work maybe consider support me at paypal
//==== sader1992@gmail.com
//============================================================
//============================================================
-	script	sader_Hourly_Rewards	-1,{

OnHourlyRewards:
		if(#Hourly_Ban){
			message strcharinfo(0),"[Hourly Rewards]: You did ban this char from the Hourly Rewards , Relogin to change that .";
			addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
			end;
		}
		if(.s_idle){
			if(checkidle() > .s_idle_time){
				message strcharinfo(0),"[Hourly Rewards]: No Hourly Rewards for IDLE Players!.";
				addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
				end;
			}
		}
		if(.s_vip){
			if(!vip_status(VIP_STATUS_ACTIVE)){
				message strcharinfo(0),"[Hourly Rewards]: if you are a VIP you will get Hourly Rewards!.";
				addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
				end;
			}
		}
		if(BaseLevel < .s_hourly_level){
			message strcharinfo(0),"[Hourly Rewards]: if you are level "+.s_hourly_level+" and more you will get Hourly Rewards!.";
			addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
			end;
		}
		if(.s_vinding){
			if(checkvending() & .s_vinding){
				message strcharinfo(0),"[Hourly Rewards]: No Hourly Rewards for Venders , Relogin to change that .";
				if(.s_GePard_ip){
				query_logsql("delete from `sader_variables_log` where `variable` = '#Hourly_Rewads_Check' AND `account_id`= '"+getcharid(3)+"'");
				}
				#Hourly_Rewads_Check = 0;
				#Hourly_Ban = 1;
				addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
				end;
			}
		}
		#Hourly_Rewads_Check += 1;
		if(.s_GePard_ip == 1){
			query_sql("SELECT `last_unique_id` FROM `login` WHERE `account_id` = '"+getcharid(3)+"'", .@s_last_unique_id);
			query_logsql("SELECT value FROM `sader_variables_log` WHERE unique_id = '"+.@s_last_unique_id+"' AND `variable` = '#Hourly_Rewads_Check'", .@s_GePard);
			if(.@s_GePard >= #Hourly_Rewads_Check){
				message strcharinfo(0),"[Hourly Rewards]: You already got the Hourly Rewards from this PC";
				#Hourly_Rewads_Check -= 1;
				addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
				end;
			}
			if(.@s_GePard == 0){
				query_logsql("INSERT INTO `sader_variables_log` (`unique_id`,`ip`,`variable`,`index`,`value`, `account_id`, `char_id`, `char_name`) VALUES ('"+.@s_last_unique_id+"', '"+getcharip()+"', '#Hourly_Rewads_Check', '0', '"+#Hourly_Rewads_Check+"', '"+getcharid(3)+"', '"+getcharid(0)+"', '"+strcharinfo(0)+"')");
			}
			query_logsql("Update `sader_variables_log` SET `value` = '"+#Hourly_Rewads_Check+"' WHERE `account_id`= '"+getcharid(3)+"'");
		}else if(.s_GePard_ip == 2){
			query_logsql("SELECT value FROM `sader_variables_log` WHERE ip = '"+getcharip()+"' AND `variable` = '#Hourly_Rewads_Check'", .@s_ip);
			if(.@s_ip >= #Hourly_Rewads_Check){
				message strcharinfo(0),"[Hourly Rewards]: You already got the Hourly Rewards from this IP";
				#Hourly_Rewads_Check -= 1;
				addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
				end;
			}
			if(.@s_ip == 0){
				query_logsql("INSERT INTO `sader_variables_log` (`unique_id`,`ip`,`variable`,`index`,`value`, `account_id`, `char_id`, `char_name`) VALUES ('"+.@s_last_unique_id+"', '"+getcharip()+"', '#Hourly_Rewads_Check', '0', '"+#Hourly_Rewads_Check+"', '"+getcharid(3)+"', '"+getcharid(0)+"', '"+strcharinfo(0)+"')");
			}
			query_logsql("Update `sader_variables_log` SET `value` = '"+#Hourly_Rewads_Check+"' WHERE `account_id`= '"+getcharid(3)+"'");
		}
		message strcharinfo(0),"[Hourly Rewards]: you Gain your Reward.";
		callsub Hourly_Rewads;
		addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
end;

OnHourlyBan:
	if(!#Hourly_Ban){
		message strcharinfo(0),"[Hourly Rewards]: You did ban this char from the Hourly Rewards , Relogin to change that .";
		if(.s_GePard_ip){
		query_logsql("delete from `sader_variables_log` where `variable` = '#Hourly_Rewads_Check' AND `account_id`= '"+getcharid(3)+"'");
		}
		#Hourly_Rewads_Check = 0;
		#Hourly_Ban = 1;
		addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
	}
end;

OnUnHourlyBan:
	#Hourly_Ban = 0;
end;
OnPCLogoutEvent:
	if(.s_GePard_ip){
	query_logsql("delete from `sader_variables_log` where `variable` = '#Hourly_Rewads_Check' AND `account_id`= '"+getcharid(3)+"'");
	}
	if(#Hourly_Ban == 1){
		#Hourly_Ban = 0;
	}
	#Hourly_Rewads_Check = 0;
end;

OnPCLoginEvent:
	addtimer .s_time, strnpcinfo(3)+"::OnHourlyRewards";
end;

OnInit:
	.s_idle = 0; //	ban hourly rewards from idle players ? 0 no / 1 yes {if player didn't move for x time he wont get hourly reward}  , DEFAULT = 0
	.s_idle_time = 1800; //	this the time for idle in secend  , DEFAULT = 1800 (30 min)
	.s_vip = 0; //	1 = only for VIP / 0 = for everyone  , DEFAULT = 0
	.s_time = 3600000; //	1000 = 1 secend | 60000 = 1 min | 3600000 = 1 houre  , DEFAULT = 3600000 (1 hr)
	.s_hourly_level = 0; //	Minimam level to get the Houerly rewards / 0 = no minimem level  , DEFAULT 0
	.s_GePard_ip = 0; //	0 = no Gepard / 1 = Gepard / 2 = IP  , DEFAULT 0
	.s_vinding = 7; //	DEFAULT 7 / 0 = will give the reward even if vending / 1 = no normal vending / 2 = no @autotrade / 4 = no buyingstore | Example: if you want to ban normal vend and buying store you add the numbers 1+4=5
	query_logsql("CREATE TABLE IF NOT EXISTS `sader_variables_log` (`unique_id` INT( 11 ) UNSIGNED NOT NULL DEFAULT  '0',`ip` VARCHAR(100) NOT NULL,`variable` VARCHAR(32) NOT NULL, `index` INT NOT NULL, `value` INT NOT NULL,`account_id` INT NOT NULL,`char_id` INT NOT NULL,`char_name` VARCHAR(30) NOT NULL) ENGINE=MyISAM");
	bindatcmd("UnHourlyBan",strnpcinfo(3)+"::OnUnHourlyBan",99,99);
	bindatcmd("HourlyBan",strnpcinfo(3)+"::OnHourlyBan",0,99);
end;

Hourly_Rewads:
	//put the Hourly Rewards here
	#HourlyRewards += 1;
	//getitem 905,1; //	if you want to add items as reward you can add them here like that
	//getexp 10000,5000; //	if you wanna add exp as reward add them here like that
	//if(#Hourly_Rewads_Check == 5){getitem 905,1;} //	like this you can add reward for Xhr and this reward will be given for that X in this example 5 mean that the player did pass 5hr
	//if(#Hourly_Rewads_Check >= 5){getitem 905,1;} //	like this you can add reward for Xhr and this reward will be given for that X in this example 5 mean that the player did pass 5hr or more
return;
}
prontera,151,171,5	pointshop	Hourly Rewards Shop	667,#HourlyRewards,901:1,902:10,903:15,904:1,905:200
//<ITEM_ID>:<PRICE>,<ITEM_ID>:<PRICE>,<ITEM_ID>:<PRICE>,<ITEM_ID>:<PRICE>