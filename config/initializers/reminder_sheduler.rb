require 'rufus-scheduler'

scheduler = Rufus::Scheduler.new

scheduler.every("30s") do 
	puts "Testing out scheduler"
end