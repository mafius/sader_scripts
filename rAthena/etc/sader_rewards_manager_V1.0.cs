//===== rAthena Script =======================================
//= saders Reward
//===== By: ==================================================
//= Sader1992
//https://rathena.org/board/profile/30766-sader1992/
//===== Current Version: =====================================
//= 1.0
//===== Compatible With: ===================================== 
//= rAthena Project
//https://rathena.org/board/files/file/ not added yet
//https://github.com/sader1992/sader_scripts
//===== Description: =========================================
//==== all the configuration from the npc in game
//==== you can change the GM level in the start of the script
//==== if(getgroupid() >= 90 ){ change the 90 to what you want
//==== support gepard / ip / or without them
//==== support rewards for vip only
//==== support max players can get the reward
//==== support up to 10 items per 1 variable
//==== reward name will be seen from the normal players when they get the reward
//==== please send me a message if you find error
//==== if you like my work maybe consider support me at paypal
//==== sader1992@gmail.com
//============================================================
//============================================================
prontera,153,176,5	script	sader Reward	997,{
	callsub S_Rest_Value;
	if(getgroupid() >= 90 ){
		switch( select("Add Reward:Check and Remove a Reward:Claim Reward:Close") ){
					case 1: callsub S_AddReward; end;
					case 2: callsub S_CheckRewards; end;
					case 3: callsub S_ClaimReward; end;
					case 4:
		}
	}else{
		if( select("Claim Reward:Close") == 1 ){
					callsub S_ClaimReward; end;
		}	
	}
end;
S_ClaimReward:
	query_sql( "SELECT `value` FROM `sader_rewards` ", .@s_Value_Name$ );
	for(.@i = 0;.@i < getarraysize(.@s_Value_Name$);.@i++){
		deletearray .@checkp;	
		.@gpardp = 8;
		deletearray .@s_player_num;
		query_sql("SELECT `name`,`gepard`,`vip`,`max_players`,`itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10` FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "' ",.@s_name$,.@s_gepard,.@s_vip,.@s_max_players,.@s_Item_id1,.@s_Item_Amount1,.@s_Item_id2,.@s_Item_Amount2,.@s_Item_id3,.@s_Item_Amount3,.@s_Item_id4,.@s_Item_Amount4,.@s_Item_id5,.@s_Item_Amount5,.@s_Item_id6,.@s_Item_Amount6,.@s_Item_id7,.@s_Item_Amount7,.@s_Item_id8,.@s_Item_Amount8,.@s_Item_id9,.@s_Item_Amount9,.@s_Item_id10,.@s_Item_Amount10);
		query_sql("SELECT `account_id` FROM `acc_reg_num` WHERE `key` = '" +.@s_Value_Name$[.@i]+"' AND `value` = '1'",.@s_player_num);
		if(.@s_gepard == 1){
			query_sql("SELECT `last_unique_id` FROM `login` WHERE `account_id` = '"+getcharid(3)+"'", .@s_last_unique_id$);
			query_sql("SELECT account_id FROM `login` WHERE last_unique_id = '"+.@s_last_unique_id$+"'", .@s_gepard_accounts);
		}
		if(.@s_gepard == 2){
			query_sql("SELECT `last_ip` FROM `login` WHERE `account_id` = '"+getcharid(3)+"'", .@s_last_ip);
			query_sql("SELECT `account_id` FROM `login` WHERE `last_ip` = '"+.@s_last_ip+"'", .@s_ip_accounts);
		}
		if(getarraysize(.@s_player_num) < .@s_max_players){
			.@checkp += 1;
		}
		if(getd(.@s_Value_Name$[.@i]) != 1){
			.@checkp += 2;
		}
		if(.@s_vip == 1){
			if(vip_status(VIP_STATUS_ACTIVE)){
			.@checkp += 4;
			}
		}else{
			.@checkp += 4;
		}
		if(.@s_gepard == 1){
			for(.@g=0;.@g < getarraysize(.@s_gepard_accounts);.@g++){
				query_sql("SELECT `value` FROM `acc_reg_num` WHERE `account_id` = '"+.@s_gepard_accounts[.@g]+"' AND `key` = '"+.@s_Value_Name$[.@i]+"'",.@s_gepard_helper);
				if(.@s_gepard_helper == 1){
					.@gpardp -= 1;	
				}
				.@s_gepard_helper = 0;
			}
		}else if(.@s_gepard == 2){
			for(.@p=0;.@p<getarraysize(.@s_ip_accounts);.@p++){
				query_sql("SELECT `value` FROM `acc_reg_num` WHERE `account_id` = '"+.@s_ip_accounts[.@p]+"' AND `key` = '"+.@s_Value_Name$[.@i]+"'",.@s_ip_helper);
				if(.@s_ip_helper == 1){
					.@gpardp -= 1;	
				}
				.@s_ip_helper = 0;
			}
		}
		.@checkp += .@gpardp;
		if(.@checkp & 1 && .@checkp & 2 && .@checkp & 4 && .@checkp & 8){
			for(.@s=1;.@s<11;.@s++){
				if(getd(".@s_Item_id" + .@s) != 0){
					getitem getd(".@s_Item_id" + .@s),getd(".@s_Item_Amount" + .@s);
					setd .@s_Value_Name$[.@i],1;
					mes "YOU got " + .@s_name$ + " Reward.";
				}
			}
		}
	}
	mes "You got all your rewards for now.";
end;
S_CheckRewards:
	query_sql( "SELECT `value` FROM `sader_rewards` ", .@s_Value_Name$ );
	for(.@i = 0;.@i < getarraysize(.@s_Value_Name$);.@i++){
		deletearray .@s_player_num;
		query_sql("SELECT `name`,`gepard`,`vip`,`max_players`,`itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10` FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "' ",.@s_name$,.@s_gepard,.@s_vip,.@s_max_players,.@s_Item_id1,.@s_Item_Amount1,.@s_Item_id2,.@s_Item_Amount2,.@s_Item_id3,.@s_Item_Amount3,.@s_Item_id4,.@s_Item_Amount4,.@s_Item_id5,.@s_Item_Amount5,.@s_Item_id6,.@s_Item_Amount6,.@s_Item_id7,.@s_Item_Amount7,.@s_Item_id8,.@s_Item_Amount8,.@s_Item_id9,.@s_Item_Amount9,.@s_Item_id10,.@s_Item_Amount10);
		query_sql("SELECT `account_id` FROM `acc_reg_num` WHERE `key` = '" +.@s_Value_Name$[.@i]+"' AND `value` = '1'",.@s_player_num);
		mes "The Name is : " + .@s_name$ + " .";
		mes "The Value is : " + .@s_Value_Name$[.@i] + " .";
		if(.@s_gepard == 1){mes "Abuse Protection : Gepard.";}else if(.@s_gepard == 2){mes "Abuse Protection : IP.";}else{mes "Abuse Protection : No Protection.";}
		if(.@s_vip ==1){mes "VIP : ON";}else{mes "VIP : OFF";}
		mes "Max Player : " + .@s_max_players + " .";
		mes "[ " + getarraysize(.@s_player_num) + " ] has got the reward.";
		for(.@a=1;.@a<11;.@a++){
			if(getd(".@s_Item_id" + .@a) != 0){
				mes getitemname(getd(".@s_Item_id" + .@a)) + " = " + getd(".@s_Item_Amount" + .@a);
			}
		}
		switch( select("Next:Delete:close") ){
			case 1: next; break;
			case 2:	
				mes "Are You Sure ?";
					if( select("yes:no") ==1 ){
						query_sql("DELETE FROM `sader_rewards` WHERE `value` = '" +.@s_Value_Name$[.@i]+ "'"); end;
					}
			case 3:	
		}
	}
	close;
end;
S_AddReward:
	callsub S_Rest_Value;
	mes "input Name";
	mes "The reward Name will be seen by the players.";
	input @s_Name$;
	mes "Input Value";
	mes"Example : #SADER";
	input @s_Value_Name$;
	mes "Max Player";
	input @s_max_players;
	mes "Abuse Protection";
	switch( select("Gepard:IP:No Protection") ){case 1: set @s_gepard,1;break; case 2: set @s_gepard,2;break; case 3: set @s_gepard,0;break;}
	mes "VIP ?";
	switch( select("No:Yes") ){case 1: set @s_vip,0;break; case 2: set @s_vip,1;break;}
	next;
	for(.@i=1;.@i<10;.@i++){
		mes "Input ITEM ID " + .@i + " .";
		input @s_Item_id[.@i];
		next;
		mes "Input ITEM " + .@i + " Amount .";
		input @s_Item_Amount[.@i];
		next;
		callsub s_addmore;
	}
s_addmore:
	switch( select("Add More items:No More items:Close") ){
		case 1: return;
		case 2: break;
		case 3: close;
	}
	mes "Are You Sure ?";
	mes "The Name is : " + @s_Name$ + " .";
	mes "The Value is : " + @s_Value_Name$ + " .";
	if(@s_gepard == 1){mes "Abuse Protection : Gepard.";}else if(@s_gepard == 2){mes "Abuse Protection : IP.";}else{mes "Abuse Protection : No Protection.";}
	if(@s_vip ==1){mes "VIP : ON";}else{mes "VIP : OFF";}
	mes "Max Player : " + @s_max_players + " .";
	for(.@i=1;.@i<10;.@i++){
		mes getitemname(@s_Item_id[.@i]) + " = " + @s_Item_Amount[.@i];
	}
	if( select("yes:no") ==1 ){
		query_sql("INSERT INTO `sader_rewards` (`value`,`name`,`gepard`,`vip`,`max_players`, `itemid1`, `amount1`, `itemid2`, `amount2`, `itemid3`, `amount3`, `itemid4`, `amount4`, `itemid5`, `amount5`, `itemid6`, `amount6`, `itemid7`, `amount7`, `itemid8`, `amount8`, `itemid9`, `amount9`, `itemid10`, `amount10`) VALUES ('"+@s_Value_Name$+"', '"+@s_Name$+"', '"+@s_gepard+"', '"+@s_vip+"', '"+@s_max_players+"', '"+@s_Item_id[1]+"', '"+@s_Item_Amount[1]+"', '"+@s_Item_id[2]+"', '"+@s_Item_Amount[2]+"', '"+@s_Item_id[3]+"', '"+@s_Item_Amount[3]+"', '"+@s_Item_id[4]+"', '"+@s_Item_Amount[4]+"', '"+@s_Item_id[5]+"', '"+@s_Item_Amount[5]+"', '"+@s_Item_id[6]+"', '"+@s_Item_Amount[6]+"', '"+@s_Item_id[7]+"', '"+@s_Item_Amount[7]+"', '"+@s_Item_id[8]+"', '"+@s_Item_Amount[8]+"', '"+@s_Item_id[9]+"', '"+@s_Item_Amount[9]+"', '"+@s_Item_id[10]+"', '"+@s_Item_Amount[10]+"')");
		mes"Done .";																																																																											//`value`,`name`,`gepard`,`vip`,`max_players`,	@s_Name$		@s_max_players		@s_gepard		@s_vip													
	}
	callsub S_Rest_Value;
	close;
end;
S_Rest_Value:
	deletearray @s_Item_id;
	deletearray @s_Item_Amount;
	deletearray @s_Name$;
	deletearray @s_Value_Name$;
	deletearray @s_max_players;
	return;
end;
OnInit:
	query_sql("CREATE TABLE IF NOT EXISTS `sader_rewards` (`name` VARCHAR(32) NOT NULL,`value` VARCHAR(32) NOT NULL, `gepard` ENUM('0','1','2') NOT NULL, `vip` ENUM('0','1') NOT NULL,`max_players` INT NOT NULL,`itemid1` INT NOT NULL, `amount1` INT NOT NULL, `itemid2` INT NOT NULL, `amount2` INT NOT NULL, `itemid3` INT NOT NULL, `amount3` INT NOT NULL, `itemid4` INT NOT NULL, `amount4` INT NOT NULL, `itemid5` INT NOT NULL, `amount5` INT NOT NULL, `itemid6` INT NOT NULL, `amount6` INT NOT NULL, `itemid7` INT NOT NULL, `amount7` INT NOT NULL, `itemid8` INT NOT NULL, `amount8` INT NOT NULL, `itemid9` INT NOT NULL, `amount9` INT NOT NULL, `itemid10` INT NOT NULL, `amount10` INT NOT NULL , UNIQUE `value` (`value`(32))) ENGINE=MyISAM");
end;
}