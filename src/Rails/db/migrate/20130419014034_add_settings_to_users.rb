class AddSettingsToUsers < ActiveRecord::Migration
  def change

    remove_column :users, :texts_enabled
    add_column :users, :sms_enabled, :boolean, :default => :true
    add_column :users, :notification_freq, :string
    add_column :users, :email_enabled, :boolean, :default => :true

  end
end
