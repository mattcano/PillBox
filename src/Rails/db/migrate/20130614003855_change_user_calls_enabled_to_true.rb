class ChangeUserCallsEnabledToTrue < ActiveRecord::Migration
  def up
    change_column_default(:users, :calls_enabled, true)
  end

  def down
  end
end
