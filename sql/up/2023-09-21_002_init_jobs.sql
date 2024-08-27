create table jobs (
  id varchar(50) not null,
  key varchar(100) not null,
  cron varchar(50) not null,
  primary_cron varchar(50) not null,
  type varchar(50) not null,
  execution_job_object varchar(100) not null,
  scheduler_base varchar(50) not null,
  timezone_name varchar(100) not null,
  "start" timestamp without time zone not null,
	"end" timestamp without time zone not null,
  request_json_content text not null,
  created_utc timestamp without time zone not null default now(),
  updated_utc timestamp without time zone not null default now(),
  deleted boolean not null default false,
  constraint pk_jobs primary key(id)
);