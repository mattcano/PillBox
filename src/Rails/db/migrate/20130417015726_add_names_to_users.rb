class AddNamesToUsers < ActiveRecord::Migration
  def change
    add_column :users, :name, :string
    add_column :users, :accepted_invitation, :boolean, :default => :false
    add_column :users, :phone_number, :integer
    add_column :users, :phone_is_cell, :boolean, :default => :true
    add_column :users, :texts_enabled, :boolean, :default => :false
    add_column :users, :calls_enabled, :boolean, :default => :false
  end
end
