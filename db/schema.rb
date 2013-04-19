# encoding: UTF-8
# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended to check this file into your version control system.

ActiveRecord::Schema.define(:version => 20130419014034) do

  create_table "coaches_dependents", :force => true do |t|
    t.integer  "dependent_id"
    t.integer  "coach_id"
    t.datetime "created_at",   :null => false
    t.datetime "updated_at",   :null => false
  end

  create_table "medications", :force => true do |t|
    t.string   "name"
    t.string   "drug"
    t.integer  "dosage_quant"
    t.string   "dosage_size"
    t.integer  "frequency"
    t.string   "period"
    t.integer  "user_id"
    t.integer  "bottle_size"
    t.text     "notes"
    t.datetime "created_at",   :null => false
    t.datetime "updated_at",   :null => false
  end

  create_table "reminders", :force => true do |t|
    t.datetime "date"
    t.integer  "user_id"
    t.integer  "medication_id"
    t.text     "message"
    t.datetime "created_at",    :null => false
    t.datetime "updated_at",    :null => false
  end

  create_table "users", :force => true do |t|
    t.string   "email",                  :default => "",    :null => false
    t.string   "encrypted_password",     :default => "",    :null => false
    t.string   "reset_password_token"
    t.datetime "reset_password_sent_at"
    t.datetime "remember_created_at"
    t.integer  "sign_in_count",          :default => 0
    t.datetime "current_sign_in_at"
    t.datetime "last_sign_in_at"
    t.string   "current_sign_in_ip"
    t.string   "last_sign_in_ip"
    t.datetime "created_at",                                :null => false
    t.datetime "updated_at",                                :null => false
    t.string   "name"
    t.boolean  "accepted_invitation",    :default => false
    t.integer  "phone_number"
    t.boolean  "phone_is_cell",          :default => true
    t.boolean  "calls_enabled",          :default => false
    t.boolean  "sms_enabled",            :default => true
    t.string   "notification_freq"
    t.boolean  "email_enabled",          :default => true
  end

  add_index "users", ["email"], :name => "index_users_on_email", :unique => true
  add_index "users", ["reset_password_token"], :name => "index_users_on_reset_password_token", :unique => true

end
