create extension if not exists "uuid-ossp";

create table cron_expressions (
  id uuid default uuid_generate_v4(),
  code varchar(50) not null,
  name varchar(255) not null,
  cron varchar(999) not null,
  created_utc timestamp without time zone not null default now(),
  updated_utc timestamp without time zone not null default now(),
  deleted boolean not null default false,
  constraint pk_cron_expressions primary key(id),
  constraint u_code unique(code)
);