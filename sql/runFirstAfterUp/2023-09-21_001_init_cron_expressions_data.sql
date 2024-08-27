delete from cron_expressions;

insert into cron_expressions(code, name, cron) values('2_seconds', 'Every 2 seconds', '0/2 * * * * ?');
insert into cron_expressions(code, name, cron) values('5_seconds', 'Every 5 seconds', '0/5 * * * * ?');
insert into cron_expressions(code, name, cron) values('10_seconds', 'Every 10 seconds', '0/10 * * * * ?');
insert into cron_expressions(code, name, cron) values('15_seconds', 'Every 15 seconds', '0/15 * * * * ?');
insert into cron_expressions(code, name, cron) values('30_seconds', 'Every 30 seconds', '0/30 * * * * ?');
insert into cron_expressions(code, name, cron) values('1_minute', 'Every minute', '0 */1 * * * ?');
insert into cron_expressions(code, name, cron) values('2_minutes', 'Every 2 minutes', '0 */2 * * * ?');
insert into cron_expressions(code, name, cron) values('5_minutes', 'Every 5 minutes', '0 */5 * * * ?');
insert into cron_expressions(code, name, cron) values('10_minutes', 'Every 10 minutes', '0 */10 * * * ?');
insert into cron_expressions(code, name, cron) values('15_minutes', 'Every 15 minutes', '0 */15 * * * ?');
insert into cron_expressions(code, name, cron) values('30_minutes', 'Every 30 minutes', '0 */30 * * * ?');
insert into cron_expressions(code, name, cron) values('1_hour', 'Every hour', '0 0 */1 * * ?');
insert into cron_expressions(code, name, cron) values('2_hours', 'Every 2 hours', '0 0 */2 * * ?');
insert into cron_expressions(code, name, cron) values('5_hours', 'Every 5 hours', '0 0 */5 * * ?');
insert into cron_expressions(code, name, cron) values('10_hours', 'Every 10 hours', '0 0 */10 * * ?');
insert into cron_expressions(code, name, cron) values('1_day', 'At 12:00 AM', '0 0 0 */1 * ?');
insert into cron_expressions(code, name, cron) values('2_days', 'At 12:00 AM, every 2 days', '0 0 0 */2 * ?');
insert into cron_expressions(code, name, cron) values('5_days', 'At 12:00 AM, every 5 days', '0 0 0 */5 * ?');
insert into cron_expressions(code, name, cron) values('1_week', 'At 12:00 AM, only on Sunday', '0 0 0 ? * SUN');
insert into cron_expressions(code, name, cron) values('1_month', 'At 12:00 AM, on day 1 of the month', '0 0 0 1 * ?');
insert into cron_expressions(code, name, cron) values('1_year', 'At 12:00 AM, on day 1 of the month, only in January', '0 0 0 1 1 ?');